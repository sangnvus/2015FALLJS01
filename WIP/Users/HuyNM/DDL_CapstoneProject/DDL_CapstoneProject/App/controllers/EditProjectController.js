﻿"use strict";

app.controller("EditProjectController", function ($scope, $filter, $location, toastr, $routeParams, ProjectService, categories, project) {

    // Get project record
    $scope.Project = project.data.Data;

    // Get categories
    $scope.Categories = categories.data.Data;

    // Checking project categoy
    var categoryIndex = 0;
    for (var i = 0; i < $scope.Categories.length; i++) {
        if ($scope.Categories[i].CategoryId == $scope.Project.CategoryId) {
            categoryIndex = i;
            console.log("index i : " + categoryIndex);
            break;
        }
    };

    // Set selected project category
    $scope.selectedOption = $scope.Categories[categoryIndex];

});