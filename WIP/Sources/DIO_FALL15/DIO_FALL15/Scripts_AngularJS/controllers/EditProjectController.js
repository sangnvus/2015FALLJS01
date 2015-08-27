"use strict";

app.directive('fileModel', ['$parse',
          function ($parse) {
              return {
                  restrict: 'A',
                  link: function (scope, element, attrs) {
                      var model = $parse(attrs.fileModel);
                      var modelSetter = model.assign;
                      element.bind('change', function () {
                          scope.$apply(function () {
                              modelSetter(scope, element[0].files[0]);
                          });
                      });
                  }
              };
          }
]);

app.controller("EditProjectController", function ($scope, $location, $routeParams, ProjectService) {

    var Id = $routeParams.id;
    function loadProjectRecord() {
        var promiseGetProject = ProjectService.getProject(Id);
        promiseGetProject.then(
            function (result) {
                $scope.Project = result.data;
            },
            function (error) {
                $scope.error = error;
            });
    }

    loadProjectRecord();

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
            CategoryId: $scope.Project.CategoryId,
            Description: $scope.Project.Description,
            Deadline: $scope.Project.Deadline,
            TargetFund: $scope.Project.TargetFund,
            ImageLink: fileName
        };

        var promisePut = ProjectService.EditProject($scope.Project.Id, $scope.Project);

        promisePut.then(
            function (result) {
                if (result.status == "200") {
                    $scope.Success = result.data;
                    $scope.EditProfileForm.$setPristine();
                }
            },
            function (error) {
                if (error.data.Message != null) {
                    $scope.Error = error.data.Message;
                } else {
                    $scope.Error = error.data;
                }

            });
    };

});