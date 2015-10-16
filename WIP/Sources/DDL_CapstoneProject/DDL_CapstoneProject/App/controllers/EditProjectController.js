"use strict";

app.controller("EditProjectController", function ($scope, $filter, $location, toastr, CommmonService, $routeParams, ProjectService, categories, project, fileReader, SweetAlert) {
    // initial newReward
    $scope.NewReward = {};
    // initial newUpdateLog
    $scope.NewUpdateLog = {};

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
    if ($scope.Project.ExpireDate != null) {
        $scope.Project.ExpireDate = new Date($filter('date')($scope.Project.ExpireDate, "yyyy-MM-dd"));
    }

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
        console.log("story: " + $scope.ProjectStory);
        console.log("des: " + $scope.ProjectStory.Description);
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
                        if ($scope.RewardPKgs[i].EstimatedDelivery != null) {
                            $scope.RewardPKgs[i].EstimatedDelivery = new Date($filter('date')($scope.RewardPKgs[i].EstimatedDelivery, "yyyy-MM-dd"));
                        }
                        if ($scope.RewardPKgs[i].Quantity > 0) {
                            $scope.RewardPKgs[i].LimitQuantity = true;
                        }
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

        // Check if rewardPkg is not limit quantity
        for (var i = 0; i < $scope.RewardPKgs.length; i++) {
            if ($scope.RewardPKgs[i].LimitQuantity == false) {
                $scope.RewardPKgs[i].Quantity = 0;
            }
        };

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

    // Prevent switch tab if tab's invalid
    $('#tablist a').click(function (e) {
        if (!angular.equals($scope.originalProjectBasic, $scope.Project)) {
            $scope.checkEditProjectBasic($scope.BasicForm);
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            $scope.checkEditReward($scope.rewardForm);
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            $scope.checkEditUpdateLog($scope.updateForm);
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            $scope.checkEditStory($scope.storyForm);
            e.stopImmediatePropagation();
        }

        // Get the tab want to move to
        $scope.thisTab = $(this);
    });

    // If tab basic is dirty
    $scope.checkEditProjectBasic = function (form) {
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
                    $scope.thisTab.tab('show');
                }
            });
    };

    // If tab reward is dirty
    $scope.checkEditReward = function (form) {
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
                } else {
                    SweetAlert.swal("Cancelled", "Reward is safe :)", "error");
                    $scope.RewardPKgs = angular.copy($scope.originalReward);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    $scope.thisTab.tab('show');
                }
            });
    };

    // If tab update log is dirty
    $scope.checkEditUpdateLog = function (form) {
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
                    // Switch tab
                    $scope.thisTab.tab('show');
                } else {
                    SweetAlert.swal("Cancelled", "Project's update log is safe :)", "error");
                    $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    $scope.thisTab.tab('show');
                }
            });
    };

    // If tab story is dirty
    $scope.checkEditStory = function (form) {
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
                } else {
                    SweetAlert.swal("Cancelled", "Project's story is safe :)", "error");
                    $scope.ProjectStory = angular.copy($scope.originalStory);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    $scope.thisTab.tab('show');
                }
            });
    };

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
        $scope.Project.Status = "pending";
        // Put update project
        var promisePut = ProjectService.submitProject($scope.Project);

        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Submit thành công!', 'Thành công!');
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