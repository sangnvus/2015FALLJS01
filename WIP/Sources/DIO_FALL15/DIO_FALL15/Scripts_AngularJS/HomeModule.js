"use strict";

var app = angular.module("App", ["ngRoute"]);

//app.factory("ShareData", function () {
//    return { value: 0 }
//});

// Show Routing.
app.config(["$routeProvider", "$locationProvider", function ($routeProvider, $locationProvider) {
    $routeProvider.when("/showproduct",
        {
            templateUrl: "/Products/ShowProducts",
            controller: "ShowProductsController"
        });
    $routeProvider.when("/addProduct",
        {
            templateUrl: "Products/AddNewProduct",
            controller: "AddProductController"
        });
    $routeProvider.when("/editProduct",
        {
            templateUrl: "Products/EditProduct",
            controller: "EditProductController"
        });
    $routeProvider.when("/deleteProduct",
        {
            templateUrl: "Products/DeleteProduct",
            controller: "DeleteProductController"
        });
    $routeProvider.otherwise(
        {
            redirectTo: "/"
        });

    //$locationProvider.html5Mode(false).hashPrefix("!");
}]);