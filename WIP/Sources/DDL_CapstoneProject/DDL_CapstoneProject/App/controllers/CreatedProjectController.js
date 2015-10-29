//"use strict";

app.controller('CreatedProjectController', function ($scope, projects, $filter, ProjectService, toastr) {
    $scope.ListCreatedProject = projects.data.Data;

    $scope.ListCreatedProjectApproved = $filter('filter')($scope.ListCreatedProject, { Status: "4"});
    $scope.ListCreatedProjectExpired = $filter('filter')($scope.ListCreatedProject, { Status: "6" });
    $scope.ListCreatedProjectDraft = $filter('filter')($scope.ListCreatedProject, { Status: "1" });
    $scope.ListCreatedProjectRejected = $filter('filter')($scope.ListCreatedProject, { Status: "3" });
    $scope.ListCreatedProjectPending = $filter('filter')($scope.ListCreatedProject, { Status: "2" });
    $scope.ListCreatedProjectSuspended = $filter('filter')($scope.ListCreatedProject, { Status: "5" });

    $scope.ListCreatedProjectDraft = $scope.ListCreatedProjectDraft.concat($scope.ListCreatedProjectRejected).concat($scope.ListCreatedProjectPending);
    $scope.ListCreatedProjectExpired = $scope.ListCreatedProjectExpired.concat($scope.ListCreatedProjectSuspended);

    $scope.checkEmpty = $scope.ListCreatedProject.length;
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
