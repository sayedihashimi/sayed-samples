This file explains how Visual Studio created the project.

The following tools were used to generate this project:
- Angular CLI (ng)

The following steps were used to generate this project:
- Create Angular project with ng: `ng new AngularJSPS --defaults --skip-install --skip-git --no-standalone `.
- Update angular.json with port.
- Create project file (`AngularJSPS.esproj`).
- Create `launch.json` to enable debugging.
- Update package.json to add `jest-editor-support`.
- Update `start` script in `package.json` to specify host.
- Add `karma.conf.js` for unit tests.
- Update `angular.json` to point to `karma.conf.js`.
- Add project to solution.
- Write this file.
