"use strict";

var app = angular.module("HomeApp", ["ngRoute", "ngCookies"]);



// Show Routing.
app.config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
    $routeProvider.when("/",
        {
            templateUrl: "/Home/ShowProjects",
            controller: "ShowProjectsController"
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
    $routeProvider.when("/project/:id",
        {
            templateUrl: "Project/ProjectDetail",
            controller: "ProjectDetailController"
        });
    $routeProvider.when("/yourproject",
        {
            templateUrl: "Project/ShowAllCurrentUserProject",
            controller: "CurrentUserProjectsController"
        });
    $routeProvider.when("/backedproject",
        {
            templateUrl: "Project/BackedProjects",
            controller: "ShowBackedProjects"
        });
    $routeProvider.when("/edit/:id",
        {
            templateUrl: "Project/Edit",
            controller: "EditProjectController"
        });
    $routeProvider.otherwise(
        {
            redirectTo: "/"
        });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]);