# FootballSimulator
FootballSimulator is a C# Blazor web application that simulates football matches between teams. It allows users to create teams, set game and environment attributes, and simulate matches to see how different strategies affect the outcome.

## Features
- Create and manage football teams with customizable attributes.

## Front-end Development
- Built with Blazor for a responsive and interactive user interface.
- Vite is used to compile and bundle front-end assets for optimal performance.
- For first time setup (if you do not see a package.json, run the following commands in the `FootballSimulator/Client` directory:
  ```bash
  npm install
  npm init -y
  npm install vite sass --save-dev
  ```
  Install the NPM Task Runner extension in Visual Studio Code to run Vite tasks directly from the IDE.  This is required for modern Visual Studio (past 2022) versions that do not have built-in NPM support.
  You should then be able to run the "dev" and "build" tasks from the Task Runner Explorer.
