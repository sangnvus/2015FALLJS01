﻿"use strict";

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

app.directive("ngFileSelect", function () {
    return {
        link: function ($scope, el) {
            el.bind("change", function (e) {
                $scope.file = (e.srcElement || e.target).files[0];
                $scope.getFile();
            })
        }
    }
})

app.controller('CreateProjectController', function ($scope, $location, ProjectService,
    fileReader, AccountService) {

    // Load categories
    function loadAllCategoriesRecords() {
        var promiseGetCategory = ProjectService.getCategories();
        promiseGetCategory.then(
            function (result) {
                $scope.Categories = result.data;

                // Set selected project category
                $scope.selectedOption = $scope.Categories[0];
            },
            function (error) {
                $scope.error = error;
            });
    }

    loadAllCategoriesRecords();


    // Load current user data
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

    // Preview image file
    $scope.getFile = function () {
        $scope.progress = 0;
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          $scope.imageSrc = result;
                      });
    };

    $scope.$on("fileProgress", function (e, progress) {
        $scope.progress = progress.loaded / progress.total;
    });

    // Get file image name
    var fileName;
    $('#fileSelected').on('change', function (evt) {
        var files = $(evt.currentTarget).get(0).files;


        if (files.length > 0) {
            fileName = files[0].name;
            alert(files[0].size);
        }
    });

    // Set min deadline : current time + 10 days
    var minDate = new Date(new Date().getTime() + (10 * 24 * 60 * 60 * 1000));
    var minDateDD = minDate.getDate();
    var minDateMM = minDate.getMonth() + 1;
    var minDateYY = minDate.getFullYear();

    if (parseInt(minDateDD) < 10) {
        minDateDD = "0" + minDateDD
    }
    if (parseInt(minDateMM) < 10) {
        minDateMM = "0" + minDateMM
    }

    $("#Deadline").attr({
        "min": minDateYY + "-" + minDateMM + "-" + minDateDD
    });

    $scope.save = function () {

        // Uploade file -> Call service
        var file = $scope.myFile;
        ProjectService.uploadBulkUserFileToUrl(file);

        console.log($scope.selectedOption.CategoryId);

        // Update project category
        $scope.Category = $scope.selectedOption.CategoryId;

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
            $location.path('/yourproject').replace();
        },
              function (errorPl) {
                  $scope.error = 'failure loading project', errorPl;
              });
    };

});