"use strict";

app.controller('CreateProjectController', function ($scope, ProjectService) {

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
    
    $scope.save = function () {

        var Project = {
            Title: $scope.Title,
            CategoryId: $scope.Category,
            Description: $scope.Description,
            Deadline: $scope.Deadline,
            TargetFund: $scope.TargetFund,
            UserId: "1",
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