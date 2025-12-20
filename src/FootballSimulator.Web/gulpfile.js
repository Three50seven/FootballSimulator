/// <binding ProjectOpened='sass:watch' />
const gulp = require('gulp');
const exec = require('child_process').exec;

function runNPMCommand(command) {
    return (cb) => {
        // Execute the command: 'npm run sass:watch'
        // This runs 'vite build --watch'
        exec(command, (err, stdout, stderr) => {
            console.log(stdout);
            console.error(stderr);
            // We usually don't call cb(err) for a continuous watch task,
            // but for VS Task Runner to start, we execute it.
            // Note: This task will run until you stop it in Task Runner Explorer.
        });
    }
}

// Define the task that runs your npm script
gulp.task('sass:watch', runNPMCommand('npm run sass:watch'));

// The binding comment at the top tells VS to run this task automatically.