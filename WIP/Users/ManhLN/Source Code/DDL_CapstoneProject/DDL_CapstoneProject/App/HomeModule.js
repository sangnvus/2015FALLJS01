﻿"use strict";
var service = angular.module("DDLService", []);
var directive = angular.module("DDLDirective", []);
var app = angular.module("ClientApp", ["ngRoute", "ngAnimate", "DDLService", "DDLDirective"]);

// Show Routing.
app.config(["$routeProvider", function ($routeProvider) {
    $routeProvider.when("/home",
        {
            redirectTo: "/"
        });
    $routeProvider.when("/",
        {
            templateUrl: "/ClientPartial/Home",
            controller: 'HomeController',
            resolve: {
                slides: ['SlideService', function (SlideService) {
                    return SlideService.getSlides();
                }]
            }
        });
    $routeProvider.when("/register",
        {
            templateUrl: "ClientPartial/Register"
        });
    $routeProvider.when("/register_success",
        {
            templateUrl: "ClientPartial/RegisterSuccess"
        });
    $routeProvider.when("/create",
        {
            templateUrl: "/ClientPartial/CreateProject",
            controller: "CreateProjectController"
        });
    $routeProvider.otherwise(
        {
            redirectTo: "/"
        });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]);

app.run(['$rootScope', '$window', '$anchorScroll', function ($rootScope, $window, $anchorScroll) {
    //$root.$on('$routeChangeError', function (e, curr, prev) {
    //    e.preventDefault();
    //    $window.location.href = "http://localhost:14069/Account/Login";
    //});

    // Base Url of web app.
    $rootScope.BaseUrl = angular.element($('#BaseUrl')).val();

    // Scroll top when route change.
    $rootScope.$on("$locationChangeStart", function () {
        $anchorScroll();
    });
}]);