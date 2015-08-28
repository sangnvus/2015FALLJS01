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

                // Load categories records
                loadAllCategoriesRecords();
            },
            function (error) {
                $scope.error = error;
            });
    }
    loadProjectRecord();

    // Function load all categories
    function loadAllCategoriesRecords() {
        var promiseGetCategory = ProjectService.getCategories();
        promiseGetCategory.then(
            function (result) {
                $scope.Categories = result.data;

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
            },
            function (error) {
                $scope.error = error;
            });
    }
    

    // Get file image name
    var fileName;
    $('#fileSelected').on('change', function (evt) {
        var files = $(evt.currentTarget).get(0).files;
        if (files.length > 0) {
            fileName = files[0].name;

            // Update project image file name
            $scope.Project.ImageLink = fileName;
        }
    });

    $scope.save = function () {
        // Call upload image file service
        var file = $scope.fileURL;
        ProjectService.uploadBulkUserFileToUrl(file);

        // Update project category
        $scope.Project.CategoryId = $scope.selectedOption.CategoryId;

        // Put update project
        var promisePut = ProjectService.EditProject($scope.Project.Id, $scope.Project);

        promisePut.then(
            function (result) {
                if (result.status == "200") {
                    $scope.Success = result.data;
                    $scope.EditProjectForm.$setPristine();
                    $location.path('/yourproject').replace();
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