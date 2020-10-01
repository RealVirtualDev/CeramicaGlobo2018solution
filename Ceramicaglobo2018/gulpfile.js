/// <binding BeforeBuild='AdminCss, AdminScripts' Clean='ClearAdmin, ClearPublic' ProjectOpened='WatchPublic, WatchAdmin' />
var gulp = require('gulp');
//var minifyJS = require('gulp-minify');
var concat = require('gulp-concat');
//var rename = require('gulp-rename');
var uglify = require('gulp-uglify');
var rimraf = require('gulp-rimraf');

gulp.task('clearCssAdmin', function () {
    return gulp.src('bundles/css/admin/*.*', { read: false }) // much faster
        .pipe(rimraf());
});

gulp.task('clearCssPublic', function () {
    return gulp.src(['bundles/css/*.css', 'bundles/css/*.map'], { read: false }) // much faster
        .pipe(rimraf());
});

gulp.task('clearScriptsPublic', function () {
    return gulp.src('bundles/scripts/*.js', { read: false }) // much faster
    .pipe(rimraf());
});

gulp.task('clearScriptsAdmin', function () {
    return gulp.src('bundles/scripts/admin/*.js', { read: false }) // much faster
        .pipe(rimraf());
});


gulp.task('scripts', function () {
    var babel = require('gulp-babel'); // il minify e uglify non supportano ES6

    return gulp.src(['content/scripts/jquery.js',
        'content/scripts/boostrap.js',
        'content/scripts/trunk8.js',
        'content/scripts/core.js'])
        .pipe(babel({ presets: ['es2015'] }))// il minify e uglify non supportano ES6
        .pipe(concat("all.js"))
        .pipe(uglify({ mangle: false }))
        .pipe(gulp.dest('bundles/scripts'));
    //gulp.watch('content/scripts/*.js', ['scripts']);

});

gulp.task('css', function () {
    var less = require('gulp-less');
    var LessAutoprefix = require('less-plugin-autoprefix');
    var autoprefix = new LessAutoprefix({ browsers: ['last 2 versions'] });
    var uglify = require('gulp-uglify');
    var csso = require('gulp-csso');
    var sourcemaps = require('gulp-sourcemaps');
   
    // less
    gulp.src('content/less/website.less')
        .pipe(less({
            plugins: [autoprefix]
        }))
        .pipe(gulp.dest('content/less'));
    // CSS
   return gulp.src(['content/less/website.css', 'content/css/*.css'])
        .pipe(concat("all.css"))
        .pipe(sourcemaps.init())
        .pipe(csso())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('bundles/css'));

    //gulp.watch('content/less/website.less', ['css']);
});


gulp.task('AdminScripts', function () {
    var babel = require('gulp-babel'); // il minify e uglify non supportano ES6

   return gulp.src(['content/scripts/jquery.js',
        'content/scripts/admin/boostrapmin.js',
        //'content/scripts/jquery.jltable.v3.js',
        //'content/scripts/jquery.jlupload.js',
        'content/scripts/trunk8.js',
        'content/scripts/admin/jlslidemenu.js',
        'content/scripts/admin/admincommon.js'
        
        ])
        .pipe(babel({ presets: ['es2015'] }))// il minify e uglify non supportano ES6
        .pipe(concat("all.js"))
        .pipe(uglify({ mangle: false }))
        .pipe(gulp.dest('bundles/scripts/admin'));
   // gulp.watch('content/scripts/admin/*.js', ['AdminScripts']);

});

gulp.task('AdminCss', function () {
    var less = require('gulp-less');
    var LessAutoprefix = require('less-plugin-autoprefix');
    var autoprefix = new LessAutoprefix({ browsers: ['last 2 versions'] });
    var uglify = require('gulp-uglify');
    var csso = require('gulp-csso');
    var sourcemaps = require('gulp-sourcemaps');

    // less
    gulp.src('content/less/admin.less')
        .pipe(less({
            plugins: [autoprefix]
        }))
        .pipe(gulp.dest('content/less'));

    // CSS
   return gulp.src(['content/css/admin/bootstrapmin.css','content/less/admin.css', 'content/css/admin/*.css'])
        .pipe(concat("all.css"))
        .pipe(sourcemaps.init())
        .pipe(csso())
        .pipe(sourcemaps.write('.'))
        .pipe(gulp.dest('bundles/css/admin'));

    
});

gulp.task("WatchAdmin", ["AdminScripts", "AdminCss"], function () {
    return gulp.watch('content/less/admin.less', ['AdminCss']);
    return gulp.watch('content/scripts/admin/*.js', ['AdminScripts']);
})

gulp.task("WatchPublic", ["scripts", "css"], function () {
   return gulp.watch('content/less/website.less', ['css']);
   return gulp.watch('content/scripts/*.js', ['scripts']);
})



gulp.task("ClearAdmin", ["clearCssAdmin", "clearScriptsAdmin"], function () {
    return "Admin Cleared";
});
gulp.task("ClearPublic", ["clearCssPublic", "clearScriptsPublic"], function () {
    return "Admin Cleared";
});