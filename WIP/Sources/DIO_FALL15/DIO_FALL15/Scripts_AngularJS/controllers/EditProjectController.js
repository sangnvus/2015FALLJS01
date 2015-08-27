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

app.controller("EditProjectController", function ($scope, $location, ShareData, ProjectService) {
    getStudent();

    function getProject() {
        var promiseGetProject = ProjectService.getProject(ShareData.value);

        promiseGetProject.then(function (pl) {
            $scope.Project = pl.data;
        },
              function (errorPl) {
                  $scope.error = 'failure loading Project', errorPl;
              });
    }

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