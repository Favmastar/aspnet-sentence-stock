import { cp, mkdir } from "node:fs/promises";

await mkdir("dist", { recursive: true });
await cp("src/index.html", "dist/index.html");
await cp("src/styles.css", "dist/styles.css");
await cp("node_modules/angular/angular.min.js", "dist/angular.min.js");
