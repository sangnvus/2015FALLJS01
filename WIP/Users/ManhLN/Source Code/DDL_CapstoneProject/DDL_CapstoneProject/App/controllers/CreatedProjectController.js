//"use strict";

app.controller('CreatedProjectController', function ($scope, projects, $filter, ProjectService, toastr) {
    $scope.ListCreatedProject = projects.data.Data;

    $scope.ListCreatedProjectApproved = $filter('filter')($scope.ListCreatedProject, { Status: "approved"});
    $scope.ListCreatedProjectExpired = $filter('filter')($scope.ListCreatedProject, { Status: "expired" });
    $scope.ListCreatedProjectDraft = $filter('filter')($scope.ListCreatedProject, { Status: "draft" });
    $scope.ListCreatedProjectRejected = $filter('filter')($scope.ListCreatedProject, { Status: "rejected" });
   
    $scope.ListCreatedProjectDraft = $scope.ListCreatedProjectDraft.concat($scope.ListCreatedProjectRejected);

    $scope.checkEmptyApproved = $scope.ListCreatedProjectApproved.length;

    $scope.checkEmptyExpired = $scope.ListCreatedProjectExpired.length;
        

    $scope.checkEmptyDraft = $scope.ListCreatedProjectDraft.length;

    $scope.deleteDraft = function (index) {
        var promiseDeleteDraft = ProjectService.deleteDraft($scope.ListCreatedProjectDraft[index].ProjectID);
       
        promiseDeleteDraft.then(
            function (result) {

                if (result.data.Status === "success") {
                    $scope.ListCreatedProjectDraft.splice(index, 1);
                    toastr.success('Xóa thành công!', 'Thành công!');
                } else {
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    };


});
