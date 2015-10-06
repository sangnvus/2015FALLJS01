"use strict";

app.controller('CreateProjectController', function ($scope, $location, ProjectService, categories){
    // Get categories
    $scope.Categories = categories.data.Data;
    // Set selected project category
    $scope.selectedOption = $scope.Categories[0];

    $scope.Error = null;
    $scope.save = function () {
        $scope.Project.CategoryId = $scope.selectedOption.CategoryID;

        var promisePost = ProjectService.createProject($scope.Project);

        promisePost.then(
            function (result) {
                if (result.data === "Success") {
                    alert("Add project successfully");
                    //$location.path('/edit').replace();
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
            });
    }

});