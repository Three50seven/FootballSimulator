using Microsoft.AspNetCore.Mvc;
using Common.Core;
using Common.Core.Domain;
using Common.Core.Validation;
using System.Net;

namespace Common.AspNetCore.Mvc
{
    public abstract class BaseController : Controller
    {
        protected virtual void AddErrorToModel(string errorMessage)
        {
            ModelState.AddModelError("", errorMessage);
        }

        protected virtual void AddValidationErrorsToModel(BrokenRulesList brokenRules)
        {
            ModelState.AddValidationRuleErrors(brokenRules);
        }

        protected virtual IActionResult Error()
        {
            return Error(error: null!);
        }

        protected virtual IActionResult Error(string message)
        {
            return Error(new ErrorInfo(message));
        }

        protected virtual IActionResult Error(ErrorInfo error)
        {
            if (Request.IsAjax())
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Json(error);
            }
            else
                return View("Error", error);
        }

        /// <summary>
        /// Control the transaction (rollback) status utilizating TempData when action is dressed with <see cref="UnitOfWorkTransactionAttribute"/>. 
        /// </summary>
        protected virtual void SetTransactionToRollback()
        {
            TempData.SetTransactionStatus(rollback: true);
        }

        /// <summary>
        /// Control the transaction (commit) status utilizating TempData when action is dressed with <see cref="UnitOfWorkTransactionAttribute"/>. 
        /// </summary>
        protected virtual void SetTransactionToCommit()
        {
            TempData.SetTransactionStatus(rollback: false);
        }

        /// <summary>
        /// Processes a given <see cref="CommandResult"/> of type <typeparamref name="T"/> and returns <see cref="Controller.Json(object)"/>.
        /// If <see cref="CommandResult.Succeeded"/> is false, the transaction flag is set to rollback via <see cref="SetTransactionToRollback"/>.
        /// </summary>
        /// <typeparam name="T">CommandResult type.</typeparam>
        /// <param name="result">Non-empty command result. If null, transaction will be rolled back and return basic "Failed" Json response.</param>
        /// <returns></returns>
        protected virtual JsonResult JsonCommandResult<T>(T result) where T : CommandResult
        {
            if (result == null)
            {
                SetTransactionToRollback();
                return Json("Failed");
            }

            if (!result.Succeeded)
                SetTransactionToRollback();

            return Json(result);
        }

        /// <summary>
        /// Returns bool based on <see cref="CommandResult.Succeeded"/> from <paramref name="result"/>.
        /// If false, broken rules will be added to the modelstate via <see cref="AddValidationErrorsToModel(BrokenRulesList)"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="result"></param>
        /// <returns></returns>
        protected virtual bool ProcessResult<T>(T result) where T : CommandResult
        {
            if (result == null!)
                return false;

            if (result.Succeeded)
                return true;

            AddValidationErrorsToModel(result.BrokenRules);
            return false;
        }

        protected virtual IActionResult ErrorViewFromStatusCode(string statusCode)
        {
            int httpStatusCode = GetHttpStatusCode(statusCode);
            Response.StatusCode = httpStatusCode;

            return httpStatusCode switch
            {
                (int)HttpStatusCode.NotFound => ErrorView("NotFound"),
                (int)HttpStatusCode.InternalServerError => ErrorView("InternalServerError", BuildErrorModel()),
                (int)HttpStatusCode.BadRequest => ErrorView("BadRequest"),
                (int)HttpStatusCode.Forbidden => ErrorView("Forbidden"),
                (int)HttpStatusCode.Unauthorized => ErrorView("Unauthorized"),
                _ => ErrorView("InternalServerError"),
            };
        }

        protected virtual IActionResult ErrorView(string viewName, ErrorInfo model = null)
        {
            if (Request.IsAjax())
                return PartialView(viewName, model);
            else
                return View(viewName, model);
        }

        protected virtual ErrorInfo BuildErrorModel()
        {
            ErrorInfo errorModel = null!;
            if (Guid.TryParse(HttpContext.GetUniqueRequestId(), out Guid eventId))
                errorModel = new ErrorInfo(eventId);
            return errorModel;
        }

        protected static int GetHttpStatusCode(string statusCode)
        {
            if (string.IsNullOrWhiteSpace(statusCode))
                return (int)HttpStatusCode.InternalServerError;

            int httpStatusCode = statusCode.ParseInteger(allowEmpty: true, throwError: false);
            if (httpStatusCode == 0)
                httpStatusCode = (int)HttpStatusCode.InternalServerError;

            return httpStatusCode;
        }

        protected virtual FileContentResult File(FileData file)
        {
            Guard.IsNotNull(file, nameof(file));
            return File(file.Data, file.ContentType, file.FileName);
        }
    }
}
