"use strict";

app.controller("EditProjectController", function ($scope, $filter, $location, toastr, $routeParams, ProjectService, categories, project, fileReader, SweetAlert) {
    // initial newReward
    $scope.NewReward = {};
    // initial newUpdateLog
    $scope.NewUpdateLog = {};

    // Initial tab active
    $scope.activeBasic = true;
    $scope.activeReward = false;
    $scope.activeStory = false;
    $scope.activeUpdate = false;

    // Get project's basic record
    $scope.Project = project.data.Data;

    // Get categories
    $scope.Categories = categories.data.Data;

    // Checking project categoy
    var categoryIndex = 0;
    for (var i = 0; i < $scope.Categories.length; i++) {
        if ($scope.Categories[i].CategoryID == $scope.Project.CategoryID) {
            categoryIndex = i;
            break;
        }
    };
    // Set selected project category
    $scope.selectedOption = $scope.Categories[categoryIndex];

    // convert datetime to date
    $scope.Project.ExpireDate = new Date($filter('date')($scope.Project.ExpireDate, "yyyy-MM-dd"));

    // Copy original project's basic
    $scope.originalProjectBasic = angular.copy($scope.Project);

    // Preview image file
    $scope.getFile = function (file) {
        $scope.file = file;
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          $scope.Project.ImageUrl = result;
                      });
    };

    $scope.editProjectBasic = function (form) {
        // Update project category
        $scope.Project.CategoryId = $scope.selectedOption.CategoryID;

        // Put update project
        var promisePut = ProjectService.editProject($scope.file, $scope.Project);

        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!', 'Thành công!');
                    // re-set original project basic
                    $scope.originalProjectBasic = angular.copy($scope.Project);
                    form.$setPristine();
                    form.$setUntouched();
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

    // Get project's story
    $scope.getStory = function () {
        var promiseGetProjectStory = ProjectService.getProjectStory($scope.Project.ProjectID);

        promiseGetProjectStory.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.ProjectStory = result.data.Data;
                    // Copy original project's basic
                    $scope.originalStory = angular.copy($scope.ProjectStory);
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

    // Edit project's story
    $scope.editProjectStory = function (form) {
        // Put update project's story
        var promisePutProjectStory = ProjectService.editProjectStory($scope.ProjectStory);

        promisePutProjectStory.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!', 'Thành công!');
                    // re-set original project story
                    $scope.originalStory = angular.copy($scope.ProjectStory);
                    form.$setPristine();
                    form.$setUntouched();
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

    // Get rewardPkgs record
    $scope.getReward = function () {
        var promiseGetReward = ProjectService.getRewardPkgs($scope.Project.ProjectID);

        promiseGetReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.RewardPKgs = result.data.Data;
                    // convert datetime to date
                    for (var i = 0; i < $scope.RewardPKgs.length; i++) {
                        $scope.RewardPKgs[i].EstimatedDelivery = new Date($filter('date')($scope.RewardPKgs[i].EstimatedDelivery, "yyyy-MM-dd"));
                    };
                    // Copy original project reward
                    $scope.originalReward = angular.copy($scope.RewardPKgs);
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
    // Edit rewardPkg
    $scope.updateReward = function (form) {
        console.log("reward:: " + $scope.RewardPKgs[0].Description);
        // Put update project
        var promiseUpdateReward = ProjectService.editRewardPkgs($scope.RewardPKgs);

        promiseUpdateReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!', 'Thành công!');
                    // re-set original project reward
                    $scope.originalReward = angular.copy($scope.RewardPKgs);
                    form.$setPristine();
                    form.$setUntouched();
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
    // Create a new reward
    $scope.createReward = function () {
        var promiseCreateReward = ProjectService.createReward($scope.Project.ProjectID, $scope.NewReward);

        promiseCreateReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo reward thành công!', 'Thành công!');
                    $('#addReward').modal('hide');
                    // reinitial newReward
                    $scope.NewReward = {};
                    result.data.Data.EstimatedDelivery = new Date($filter('date')(result.data.Data.EstimatedDelivery, "yyyy-MM-dd"));
                    $scope.RewardPKgs.push(result.data.Data);
                    $scope.originalReward = angular.copy($scope.RewardPKgs);
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
    // Delete a rewardPkg
    $scope.deleteReward = function (index) {
        var promiseDeleteReward = ProjectService.deleteRewardPkg($scope.RewardPKgs[index].RewardPkgID);

        promiseDeleteReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Xóa thành công!', 'Thành công!');
                    $scope.RewardPKgs.splice(index, 1);
                    $scope.originalReward = angular.copy($scope.RewardPKgs);
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

    // Get updateLog records
    $scope.getUpdateLog = function () {
        // Put update project
        var promiseGetUpdateLog = ProjectService.getUpdateLogs($scope.Project.ProjectID);

        promiseGetUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.UpdateLogs = result.data.Data;
                    // convert datetime to date
                    for (var i = 0; i < $scope.UpdateLogs.length; i++) {
                        $scope.UpdateLogs[i].CreatedDate = new Date($filter('date')($scope.UpdateLogs[i].CreatedDate, "yyyy-MM-dd"));
                    };
                    // Copy original update log
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
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
    // Edit updateLog
    $scope.editUpdateLog = function (form) {
        // Put update project
        var promiseEditUpdateLog = ProjectService.editUpdateLogs($scope.UpdateLogs);

        promiseEditUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa updateLog thành công!', 'Thành công!');
                    // re-set original update log
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
                    form.$setPristine();
                    form.$setUntouched();
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
    // Create a new updateLog
    $scope.createUpdateLog = function () {
        var promiseCreateUpdateLog = ProjectService.createUpdateLog($scope.Project.ProjectID, $scope.NewUpdateLog);

        promiseCreateUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo updateLog thành công!', 'Thành công!');
                    $('#CreateUpdateLogForm').modal('hide');
                    // reinitial newUpdateLog
                    $scope.NewUpdateLog = {};
                    result.data.Data.CreatedDate = new Date($filter('date')(result.data.Data.CreatedDate, "yyyy-MM-dd"));
                    $scope.UpdateLogs.push(result.data.Data);
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
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

    // Delete a updateLog
    $scope.deleteUpdateLog = function (index) {
        var promiseDeleteUpdateLog = ProjectService.deleteUpdateLog($scope.UpdateLogs[index].UpdateLogID);

        promiseDeleteUpdateLog.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Xóa thành công!', 'Thành công!');
                    $scope.UpdateLogs.splice(index, 1);
                    $scope.originalUpdateLog = angular.copy($scope.UpdateLogs);
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

    // Check before switch tab
    $scope.preventToBasic = function (event, rewardForm, updateForm, storyForm) {
        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            $scope.checkEditReward(rewardForm, "basic");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            $scope.checkEditUpdateLog(updateForm, "basic");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            $scope.checkEditStory(storyForm, "basic");
            event.preventDefault();
        }
    };

    $scope.preventToReward = function (event, basicForm, updateForm, storyForm) {
        if (!angular.equals($scope.originalProjectBasic, $scope.Project)) {
            $scope.checkEditProjectBasic(basicForm, "reward");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            $scope.checkEditUpdateLog(updateForm, "reward");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            $scope.checkEditStory(storyForm, "reward");
            event.preventDefault();
        }

        $scope.getReward();
    };

    $scope.preventToUpdateLog = function (event, basicForm, rewardForm, storyForm) {
        if (!angular.equals($scope.originalProjectBasic, $scope.Project)) {
            $scope.checkEditProjectBasic(basicForm, "update");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            $scope.checkEditReward(rewardForm, "update");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            $scope.checkEditStory(storyForm, "update");
            event.preventDefault();
        }

        $scope.getUpdateLog();
    };

    $scope.preventToStory = function (event, basicForm, rewardForm, updateForm) {
        if (!angular.equals($scope.originalProjectBasic, $scope.Project)) {
            $scope.checkEditProjectBasic(basicForm, "story");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            $scope.checkEditReward(rewardForm, "story");
            event.preventDefault();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            $scope.checkEditUpdateLog(updateForm, "story");
            event.preventDefault();
        }

        $scope.getStory();
    };

    // If tab basic is dirty
    $scope.checkEditProjectBasic = function (form, toTab) {
        SweetAlert.swal({
            title: "You have changed something!",
            text: "Project's basic will be edit!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Edit it!",
            cancelButtonText: "No, cancel plx!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's basic has been edited.", "success");
                    $scope.editProjectBasic(form);
                    // Switch tab
                    //$scope.changeTab(toTab);
                } else {
                    SweetAlert.swal("Cancelled", "Project's basic is safe :)", "error");
                    $scope.Project = angular.copy($scope.originalProjectBasic);
                    form.$setPristine();
                    form.$setUntouched();
                    //reset file field
                    var resetImg = $("#imgSelected");
                    resetImg.replaceWith(resetImg = resetImg.clone(true));
                    $scope.file = null;
                    // Switch tab
                    //$scope.changeTab(toTab);
                }
            });
    };

    // If tab reward is dirty
    $scope.checkEditReward = function (form, toTab) {
        SweetAlert.swal({
            title: "You have changed something!",
            text: "Project's reward will be edit!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Edit it!",
            cancelButtonText: "No, cancel plx!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Reward has been edited.", "success");
                    $scope.updateReward(form);
                    // Switch tab
                    //$scope.changeTab(toTab);
                } else {
                    SweetAlert.swal("Cancelled", "Reward is safe :)", "error");
                    $scope.RewardPKgs = angular.copy($scope.originalReward);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    //$scope.changeTab(toTab);
                }
            });
    };

    // If tab update log is dirty
    $scope.checkEditUpdateLog = function (form, toTab) {
        SweetAlert.swal({
            title: "You have changed something!",
            text: "Project's update log will be edit!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Edit it!",
            cancelButtonText: "No, cancel plx!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's update log has been edited.", "success");
                    $scope.editUpdateLog(form);
                    //$scope.changeTab(toTab);
                } else {
                    SweetAlert.swal("Cancelled", "Project's update log is safe :)", "error");
                    $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
                    form.$setPristine();
                    form.$setUntouched();
                    //$scope.changeTab(toTab);
                }
            });
    };

    // If tab story is dirty
    $scope.checkEditStory = function (form, toTab) {
        SweetAlert.swal({
            title: "You have changed something!",
            text: "Project's story will be edit!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, Edit it!",
            cancelButtonText: "No, cancel plx!",
            closeOnConfirm: true,
            closeOnCancel: true
        },
            function (isConfirm) {
                if (isConfirm) {
                    SweetAlert.swal("Edited!", "Project's story has been edited.", "success");
                    $scope.editProjectStory(form);
                    //$scope.changeTab(toTab);
                } else {
                    SweetAlert.swal("Cancelled", "Project's story is safe :)", "error");
                    $scope.ProjectStory = angular.copy($scope.originalStory);
                    form.$setPristine();
                    form.$setUntouched();
                    //$scope.changeTab(toTab);
                }
            });
    };

    //$scope.changeTab = function(toTab) {
    //    if (toTab === "update") {
    //        $scope.activeUpdate = true;
    //        $scope.activeBasic = false;
    //        $scope.activeStory = false;
    //        $scope.activeReward = false;
    //    };

    //    if (toTab === "reward") {
    //        $scope.activeReward = true;
    //        $scope.activeBasic = false;
    //        $scope.activeStory = false;
    //        $scope.activeUpdate = false;
    //    };

    //    if (toTab === "basic") {
    //        $scope.activeBasic = true;
    //        $scope.activeReward = false;
    //        $scope.activeStory = false;
    //        $scope.activeUpdate = false;
    //    };

    //    if (toTab === "story") {
    //        $scope.activeStory = true;
    //        $scope.activeReward = false;
    //        $scope.activeBasic = false;
    //        $scope.activeUpdate = false;
    //    };

    //    console.log("story: " + $scope.activeStory);
    //    console.log("reward: " + $scope.activeReward);
    //    console.log("basic: " + $scope.activeBasic);
    //    console.log("update: " + $scope.activeUpdate);
    //};

    // Discard project basic
    $scope.resetProjectBasic = function (form) {
        if (form) {
            $scope.Project = angular.copy($scope.originalProjectBasic);
            form.$setPristine();
            form.$setUntouched();
        }
    };
    // Discard reward
    $scope.resetReward = function (form) {
        if (form) {
            $scope.RewardPKgs = angular.copy($scope.originalReward);
            form.$setPristine();
            form.$setUntouched();
        }
    };
    // Discard story
    $scope.resetStory = function (form) {
        if (form) {
            $scope.ProjectStory = angular.copy($scope.originalStory);
            form.$setPristine();
            form.$setUntouched();
        }
    };
    // Discard update log
    $scope.resetUpdateLog = function (form) {
        if (form) {
            $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
            form.$setPristine();
            form.$setUntouched();
        }
    };

    // submit project
    $scope.submit = function () {
        // Put update project
        var promisePut = ProjectService.editProject($scope.file, $scope.Project);

        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!', 'Thành công!');
                } else {
                    $scope.Error = result.data.Message;
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                $scope.Error = error.data.Message;
                toastr.error($scope.Error, 'Lỗi!');
            });
    }
});