"use strict";

var app = angular.module("HomeApp", ["ngRoute"]);


// Show Routing.
app.config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
    $routeProvider.when("/",
        {
            templateUrl: "/Home/ShowProjects",
            controller: "ShowProjectsController"
        });
    $routeProvider.when("/login",
        {
            templateUrl: "/Account/Login",
            controller: "ShowProjectsController"
        });
    $routeProvider.when("/register",
        {
            templateUrl: "Account/Register",
            controller: "RegisterController"
        });
    //$routeProvider.when("/deleteProduct",
    //    {
    //        templateUrl: "Products/DeleteProduct",
    //        controller: "DeleteProductController"
    //    });
    $routeProvider.otherwise(
        {
            redirectTo: "/"
        });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]);