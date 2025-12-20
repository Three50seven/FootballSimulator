import { defineConfig } from "vite";
import path from "path";
import fs from "fs";
// Import 'sass' properly for modern Node environments
import * as sass from "sass";

// Function to perform the SCSS compilation and write the file
function compileScssFiles() {
    const root = path.resolve("Components");
    const includePaths = [path.resolve(process.cwd(), "wwwroot/scss")];

    function walk(dir) {
        // Ensure the directory exists before attempting to read it
        if (!fs.existsSync(dir)) return;

        for (const file of fs.readdirSync(dir)) {
            const fullPath = path.join(dir, file);
            const stat = fs.statSync(fullPath);

            if (stat.isDirectory()) {
                walk(fullPath);
            } else if (file.endsWith(".razor.scss")) {
                const scssIn = fullPath;
                const cssOut = scssIn.replace(".razor.scss", ".razor.css");

                try {
                    // 1. Compile SCSS using Dart Sass
                    const result = sass.renderSync({
                        file: scssIn,
                        includePaths: includePaths,
                    });

                    const compiledCss = result.css.toString();

                    // 2. Write the compiled CSS directly to the disk
                    // This puts the file at Components/[path]/File.razor.css, ignoring the 'dist' folder.
                    fs.writeFileSync(cssOut, compiledCss);

                    console.log(`Compiled: ${path.relative(process.cwd(), scssIn)} -> ${path.relative(process.cwd(), cssOut)}`);

                } catch (e) {
                    // Use Rollup's warning system to report compilation errors
                    // Note: Since we are not using Rollup hooks here, a console error is fine.
                    console.error(`Error compiling ${path.relative(process.cwd(), scssIn)}:`, e.message);
                }
            }
        }
    }

    walk(root);
}


export default defineConfig({
    css: {
        preprocessorOptions: {
            scss: {
                includePaths: [path.resolve(process.cwd(), "wwwroot/scss")]
            }
        }
    },

    appType: "custom",

    build: {
        // Change the ROOT output directory from 'dist' (default) to 'wwwroot/dist'
        outDir: "wwwroot/dist",

        // Change the subdirectory for assets from 'assets' (default) to 'assets'
        // This is usually kept as 'assets' so the full path becomes: wwwroot/dist/assets
        assetsDir: "assets",
        emptyOutDir: false,

        rollupOptions: {
            input: "vite.noop.js",

            plugins: [
                {
                    name: "compile-razor-scss",

                    // The buildStart hook is still the correct place to execute this logic
                    buildStart() {
                        // Call the function that writes files directly to the filesystem
                        compileScssFiles();
                    }
                }
            ]
        }
    }
});