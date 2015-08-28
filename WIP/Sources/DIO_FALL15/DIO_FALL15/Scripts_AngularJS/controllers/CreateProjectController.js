"use strict";

//This is user upload custom directive code sample.

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

    var fileName;
    $('#fileSelected').on('change', function (evt) {
        var files = $(evt.currentTarget).get(0).files;

        if (files.length > 0) {
            fileName = files[0].name
            //console.log(files[0].name);
        }
    });

    $scope.save = function () {

        var file = $scope.myFile;
        ProjectService.uploadBulkUserFileToUrl(file);

        var Project = {
            Title: $scope.Title,
            CategoryId: $scope.Category,
            Description: $scope.Description,
            Deadline: $scope.Deadline,
            TargetFund: $scope.TargetFund,
            UserId: "1",
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