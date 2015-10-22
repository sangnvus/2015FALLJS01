//"use strict";

app.controller('CreatedProjectController', function ($scope, projects) {
    $scope.ListCreatedProject = projects.data.Data;
    //function getCreatedProject() {
    //    var promiseGet = ProjectService.getCreatedProject();

    //    promiseGet.then(
    //        function (result) {
    //            if (result.data.Status === "success") {
    //                $scope.ListCreatedProject = result.data.Data;
    //            } else {
    //                CommmonService.checkError(result.data.Type);
    //                $scope.Error = result.data.Message;
    //            }
    //        },
    //        function (error) {
    //            $scope.Error = error.data.Message;
    //        });
    //}
});
