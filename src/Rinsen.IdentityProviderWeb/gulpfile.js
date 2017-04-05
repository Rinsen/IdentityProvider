/// <binding AfterBuild='debug:ngApp' Clean='clean' />
"use strict";

var gulp = require("gulp"),
    rimraf = require("rimraf"),
    concat = require("gulp-concat"),
    cssmin = require("gulp-cssmin"),
    uglify = require("gulp-uglify");

var webroot = "./wwwroot/";

var paths = {
    js: webroot + "js/**/*.js",
    minJs: webroot + "js/**/*.min.js",
    css: webroot + "css/**/*.css",
    minCss: webroot + "css/**/*.min.css",
    concatJsDest: webroot + "js/site.min.js",
    concatNgAppJsDest: webroot + "js/site.min.js",
    concatCssDest: webroot + "css/site.min.css",
    ngApp: "./ng-apps/**/*.js",
    ngDebugApp: webroot + "js/debug/**/*.js",
    minNgApp: webroot + "js/**/*.min.js"
};

gulp.task("clean:js", function (cb) {
    rimraf(paths.concatJsDest, cb);
});

gulp.task("clean:css", function (cb) {
    rimraf(paths.concatCssDest, cb);
});

gulp.task("clean", ["clean:js", "clean:css"]);

gulp.task("min:js", function () {
    return gulp.src([paths.js, "!" + paths.minJs], { base: "." })
        .pipe(concat(paths.concatJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("min:css", function () {
    return gulp.src([paths.css, "!" + paths.minCss])
        .pipe(concat(paths.concatCssDest))
        .pipe(cssmin())
        .pipe(gulp.dest("."));
});

gulp.task("min:ngApp", function () {
    return gulp.src([paths.ngApp, "!" + paths.minNgApp], { base: "." })
        .pipe(concat(paths.concatNgAppJsDest))
        .pipe(uglify())
        .pipe(gulp.dest("."));
});

gulp.task("debug:ngApp", function () {
    return gulp.src("./ng-apps/**/*.js")
            .pipe(gulp.dest("./wwwroot/js/debug/"));
});

gulp.task("min", ["min:js", "min:css", "min:ngApp"]);
