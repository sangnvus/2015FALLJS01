﻿"use strict";

app.controller('CreateProjectController', function ($scope, ProjectService, AccountService) {

    function loadAllCategoriesRecords() {
        var promiseGetCategory = ProjectService.getCategories();
        promiseGetCategory.then(
            function (result) {
                $scope.Categories = result.data;
                $scope.Category = $scope.Categories[0];
            },
            function (error) {
                $scope.error = error;
            });
    }

    loadAllCategoriesRecords();

    function loadCurrentUserData() {
        var promiseGetProfile = AccountService.getCurrentAccount();
        promiseGetProfile.then(
            function (result) {
                $scope.User = result.data;
            },
            function (error) {
                if (error.status == 401) {
                    $window.location.href = "http://localhost:14069/Account/Login";
                } else {
                    $scope.error = error;
                }
            });
    }
    loadCurrentUserData();

    $scope.save = function () {

        // Get file image name
        var fileName;
        $('#fileSelected').on('change', function (evt) {
            var files = $(evt.currentTarget).get(0).files;

            if (files.length > 0) {
                fileName = files[0].name
            }
        });

        // Uploade file -> Call service
        var file = $scope.myFile;
        ProjectService.uploadBulkUserFileToUrl(file);

        var Project = {
            Title: $scope.Title,
            CategoryId: $scope.Category,
            Description: $scope.Description,
            Deadline: $scope.Deadline,
            TargetFund: $scope.TargetFund,
            UserId: $scope.User.Id,
            ImageLink: fileName
        };

        var promisePost = ProjectService.post(Project);


        promisePost.then(function (pl) {
            alert("Add project successfully");
        },
              function (errorPl) {
                  $scope.error = 'failure loading project', errorPl;
              });
    };

});