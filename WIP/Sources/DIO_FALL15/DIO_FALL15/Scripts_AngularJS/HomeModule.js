﻿"use strict";

var app = angular.module("HomeApp", ["ngRoute"]);


// Show Routing.
app.config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
    $routeProvider.when("/",
        {
            templateUrl: "/Home/ShowProjects",
            controller: "ShowProjectsController"
        });
    //$routeProvider.when("/addProduct",
    //    {
    //        templateUrl: "Products/AddNewProduct",
    //        controller: "AddProductController"
    //    });
    //$routeProvider.when("/editProduct",
    //    {
    //        templateUrl: "Products/EditProduct",
    //        controller: "EditProductController"
    //    });
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