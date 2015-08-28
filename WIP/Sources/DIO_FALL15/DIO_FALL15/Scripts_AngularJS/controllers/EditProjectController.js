"use strict";

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
        
        // Get image file name
        var fileName;
        $('#fileSelected').on('change', function (evt) {
            var files = $(evt.currentTarget).get(0).files;

            if (files.length > 0) {
                fileName = files[0].name
                //console.log(files[0].name);
            }
        });
        // Upload new image file
        var file = $scope.fileURL;
        var uploadUrl = "/Images";
        ProjectService.uploadFileToUrl(file, uploadUrl);

        var Project = {
            Title: $scope.Title,
            CategoryId: $scope.Category,
            Description: $scope.Description,
            Deadline: $scope.Deadline,
            TargetFund: $scope.TargetFund,
            UserId: "1",
            ImageLink: fileName
        };

        var promisePutProject = ProjectService.put($scope.Project.Id, Project);
        promisePutStudent.then(function (pl) {
            $location.path("/Home");
        },
              function (errorPl) {
                  $scope.error = 'failure loading Home', errorPl;
              });
    };

});