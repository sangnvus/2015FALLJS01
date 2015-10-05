﻿"use strict";

app.controller('CreateProjectController', function ($scope, $location, ProjectService, CategoryService){
    // Get categories
    function getCategories() {
        var promiseGetCategory = CategoryService.getCategories();
        promiseGetCategory.then(
            function (result) {
                $scope.Categories = result.data;

                // Set selected project category
                $scope.selectedOption = $scope.Categories.Data[0];
            },
            function (error) {
                $scope.error = error;
            });
    }

    // Execute function
    getCategories();

    $scope.Error = null;
    $scope.save = function () {
        $scope.Project.CategoryId = $scope.selectedOption.CategoryID;

        var promisePost = ProjectService.createProject($scope.Project);

        promisePost.then(
            function (result) {
                if (result.data === "Success") {
                    alert("Add project successfully");
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

});