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

app.controller("EditProjectController", function ($scope, $filter, $location, $routeParams, ProjectService) {

    // Load project record
    var Id = $routeParams.id;
    function loadProjectRecord() {
        var promiseGetProject = ProjectService.getProject(Id);
        promiseGetProject.then(
            function (result) {
                $scope.Project = result.data;

                // convert datetime to date
                $scope.Project.Deadline = new Date($filter('date')($scope.Project.Deadline, "yyyy-MM-dd"));
            },
            function (error) {
                $scope.error = error;
            });
    }

    loadProjectRecord();
    
    // Load category
    function loadAllCategoriesRecords() {
        var promiseGetCategory = ProjectService.getCategories();
        promiseGetCategory.then(
            function (result) {
                $scope.Categories = result.data;
            },
            function (error) {
                $scope.error = error;
            });
    }
    loadAllCategoriesRecords();

    // Get file image name
    var fileName;
    $('#fileSelected').on('change', function (evt) {
        var files = $(evt.currentTarget).get(0).files;
        if (files.length > 0) {
            fileName = files[0].name;
            $scope.Project.ImageLink = fileName;

            // Call upload service
            var file = $scope.fileURL;
            ProjectService.uploadBulkUserFileToUrl(file);
        }
    });

    $scope.save = function () {

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