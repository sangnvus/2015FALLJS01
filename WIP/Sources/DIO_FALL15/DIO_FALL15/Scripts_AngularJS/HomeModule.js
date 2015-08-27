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
            controller: "LoginController"
        });
    $routeProvider.when("/register",
        {
            templateUrl: "Account/Register",
            controller: "RegisterController"
        });
    $routeProvider.when("/create",
        {
            templateUrl: "/Project/Create",
            controller: "CreateProjectController"
        });
    $routeProvider.when("/editprofile",
        {
            templateUrl: "Account/EditProfile",
            controller: "EditProfileController"
        });
    $routeProvider.otherwise(
        {
            redirectTo: "/"
        });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]);