const { series } = require('gulp');

function defaultTask(cb) {
    // place code for your default task here
    console.log('task runner started: defaultTask');
    cb();    
}

function beforeBuild(cb) {
    console.log("before build called");
    cb();
}

function afterBuild(cb) {
    console.log("after build called");
    cb();
}

// exports.default = series(defaultTask, beforeBuild, afterBuild);
exports.default = defaultTask;
exports.beforeBuild = beforeBuild;
exports.afterBuild = afterBuild;