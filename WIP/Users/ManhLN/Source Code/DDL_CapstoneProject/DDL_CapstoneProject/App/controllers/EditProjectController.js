"use strict";

app.controller("EditProjectController", function ($scope, $filter, $sce, $location, toastr, CommmonService, $routeParams, ProjectService, categories, project, fileReader, SweetAlert) {
    // initial newReward
    $scope.NewReward = {};
    // initial newUpdateLog
    $scope.NewUpdateLog = {};
    // initial newTimeline
    $scope.newTimeline = {};
    // initail newQuestion
    $scope.newQuestion = {};
    // initial edit timeline status list
    $scope.editTimeline = {};
    // Detech create a new timeline point
    $scope.isCreateTimeline = false;



    // check error list
    $scope.errorListFlag = false;

    $scope.Project = {};
    $scope.Project.Status = "approved";

    // Get project's basic record
    $scope.Project = project.data.Data;

    // Check project's status
    $scope.AllEditable = true;
    if ($scope.Project.Status === "expired" || $scope.Project.Status === "suspended" || $scope.Project.Status === "pending") {
        $scope.AllEditable = false;
    };

    // Turn URL to trust Src
    $scope.trustSrc = function (src) {
        return $sce.trustAsResourceUrl(src);
    }

    // Embed video story url
    $scope.checkVideoURL = function () {
        var regExp = /^.*(youtu.be\/|v\/|u\/\w\/|embed\/|watch\?v=|\&v=)([^#\&\?]{11,11}).*/;
        var match = $scope.ProjectStory.VideoUrl.match(regExp);
        if (match) if (match.length >= 2) {
            $scope.ProjectStory.VideoUrl = "https://www.youtube.com/embed/" + match[2];
        }
    };

    // Get current time
    $scope.toDay = new Date($.now());

    // Set min deadline : current time + 10 days
    var minDate = new Date(new Date().getTime() + (10 * 24 * 60 * 60 * 1000));
    var minDateDD = minDate.getDate();
    var minDateMM = minDate.getMonth() + 1;
    var minDateYY = minDate.getFullYear();

    if (parseInt(minDateDD) < 10) {
        minDateDD = "0" + minDateDD;
    }

    if (parseInt(minDateMM) < 10) {
        minDateMM = "0" + minDateMM;
    }

    $("#ProjectDeadLine").attr({
        "min": minDateYY + "-" + minDateMM + "-" + minDateDD
    });

    $scope.endDate = minDateYY + "-" + minDateMM + "-" + minDateDD;

    // Get categories
    $scope.Categories = categories.data.Data;

    // Checking project categoy
    var categoryIndex = 0;
    $scope.selectedCate = function () {
        for (var i = 0; i < $scope.Categories.length; i++) {
            if ($scope.Categories[i].CategoryID == $scope.Project.CategoryID) {
                $scope.Project.CategoryName = $scope.Categories[i].Name;
                categoryIndex = i;
                break;
            }
        };
    }
    $scope.selectedCate();

    //// Reward option
    //$scope.rewardOption = [
    //    { id: '1', name: 'no reward' },
    //    { id: '2', name: 'unlimited' },
    //    { id: '3', name: 'limited' }
    //];

    // Set selected project category
    $scope.selectedOption = $scope.Categories[categoryIndex];
    $scope.originalSelectedCate = angular.copy($scope.selectedOption);

    // convert datetime to date
    if ($scope.Project.ExpireDate != null) {
        $scope.Project.ExpireDate = new Date($filter('date')($scope.Project.ExpireDate, "yyyy-MM-dd"));
    }

    // Copy original project's basic
    $scope.originalProjectBasic = angular.copy($scope.Project);

    // Detech Active tab
    $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
        $scope.activeTab = $(e.target).attr("href");
    });

    // Preview image file
    $scope.getFile = function (file, index) {
        $scope.file = file;
        fileReader.readAsDataUrl($scope.file, $scope)
                      .then(function (result) {
                          if ($scope.activeTab === "#timelineTab" && $scope.isCreateTimeline === false) {
                              $scope.Timeline[index].ImageUrl = result;
                          } else if ($scope.activeTab === "#timelineTab" && $scope.isCreateTimeline === true) {
                              $scope.newTimeline.ImageUrl = result;
                          } else {
                              $scope.Project.ImageUrl = result;
                          }
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
                    $scope.Project = result.data.Data;
                    $scope.selectedCate();
                    $scope.selectedOption = $scope.Categories[categoryIndex];
                    $scope.originalSelectedCate = angular.copy($scope.selectedOption);
                    // re-set original project basic
                    $scope.originalProjectBasic = angular.copy($scope.Project);
                    form.$setPristine();
                    form.$setUntouched();
                    $scope.file = null;
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
            if ($scope.RewardPKgs[i].LimitQuantity === false) {
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
                    if (result.data.Data.Quantity > 0) {
                        result.data.Data.LimitQuantity = true;
                    }
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
                    $('#updateModal').modal('hide');
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

    // Get timeline records
    $scope.getTimeline = function () {
        var promiseGetTimeline = ProjectService.getTimeline($scope.Project.ProjectID);

        promiseGetTimeline.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Timeline = result.data.Data;
                    // convert datetime to date
                    for (var i = 0; i < $scope.Timeline.length; i++) {
                        $scope.Timeline[i].DueDate = new Date($filter('date')($scope.Timeline[i].DueDate, "yyyy-MM-dd"));
                    };
                    // Copy original update log
                    $scope.originalTimeline = angular.copy($scope.Timeline);
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
    // Update timeline
    $scope.updateTimeline = function (index) {
        $scope.editTimeline[index] = false;

        var promiseUpdateTimeline = ProjectService.updateTimeline($scope.Timeline[index], $scope.file);

        promiseUpdateTimeline.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!', 'Thành công!');
                    // re-set original timeline
                    $scope.originalTimeline = angular.copy($scope.Timeline);
                    $scope.file = null;
                    $("#title" + index).prop('disabled', true);
                    $("#date" + index).prop('disabled', true);
                    $("#" + index).prop('disabled', true);
                    $("#desc" + index).prop('disabled', true);
                    //form.$setPristine();
                    //form.$setUntouched();
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
    // Create new timeline point
    $scope.createTimeline = function () {
        console.log("time " + $scope.newTimeline.DueDate);
        var promiseCreateTimeline = ProjectService.createTimeline($scope.Project.ProjectID, $scope.newTimeline, $scope.file);

        promiseCreateTimeline.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo reward thành công!', 'Thành công!');
                    $('#addTimeline').modal('hide');
                    // reinitial new timeline point
                    $scope.newTimeline = {};
                    result.data.Data.DueDate = new Date($filter('date')(result.data.Data.DueDate, "yyyy-MM-dd"));
                    $scope.Timeline.push(result.data.Data);
                    $scope.originalTimeline = angular.copy($scope.Timeline);
                    $scope.file = null;
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
    // Delete timeline
    $scope.deleteTimeline = function (index) {
        var promiseDeleteReward = ProjectService.deleteTimeline($scope.Timeline[index].TimelineID);

        promiseDeleteReward.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Xóa thành công!', 'Thành công!');
                    $scope.Timeline.splice(index, 1);
                    // re-set original timeline
                    $scope.originalTimeline = angular.copy($scope.Timeline);
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

    // Get question records
    $scope.getQuestion = function () {
        var promiseGetQuestion = ProjectService.getQuestion($scope.Project.ProjectID);

        promiseGetQuestion.then(
            function (result) {
                if (result.data.Status === "success") {
                    $scope.Question = result.data.Data;
                    $scope.originalQuestion = angular.copy($scope.Question);
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
    // Edit question
    $scope.updateQuestion = function (form) {
        var promiseUpdateQuestion = ProjectService.editQuestion($scope.Question);

        promiseUpdateQuestion.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Sửa thành công!', 'Thành công!');
                    // re-set original project reward
                    $scope.originalQuestion = angular.copy($scope.Question);
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
    // Create a new question
    $scope.createQuestion = function () {
        var promiseCreateQuestion = ProjectService.createQuestion($scope.Project.ProjectID, $scope.newQuestion);

        promiseCreateQuestion.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Tạo reward thành công!', 'Thành công!');
                    $('#addQuestion').modal('hide');
                    // reinitial newReward
                    $scope.newQuestion = {};
                    $scope.Question.push(result.data.Data);
                    $scope.originalQuestion = angular.copy($scope.Question);
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
    // Delete a question
    $scope.deleteQuestion = function (index) {
        var promiseDeleteQuestion = ProjectService.deleteQuestion($scope.Question[index].QuestionID);

        promiseDeleteQuestion.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Xóa thành công!', 'Thành công!');
                    $scope.Question.splice(index, 1);
                    $scope.originalQuestion = angular.copy($scope.Question);
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

    // check dirty form
    $scope.dirtyForm = function () {
        if (!angular.equals($scope.originalProjectBasic, $scope.Project) || !angular.equals($scope.originalSelectedCate, $scope.selectedOption)) {
            if ($scope.BasicForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditProjectBasic($scope.BasicForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            if ($scope.rewardForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditReward($scope.rewardForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            if ($scope.updateLogForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditUpdateLog($scope.updateLogForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            if ($scope.storyForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditStory($scope.storyForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalQuestion, $scope.Question)) {
            if ($scope.questionForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditQuestion($scope.questionForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalTimeline, $scope.Timeline)) {
            if ($scope.timelineForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditTimeline($scope.timelineForm);
            }
            e.stopImmediatePropagation();
        }
    };
    // Prevent switch tab if tab's invalid
    $('#tablist a').click(function (e) {
        if (!angular.equals($scope.originalProjectBasic, $scope.Project) || !angular.equals($scope.originalSelectedCate, $scope.selectedOption)) {
            if ($scope.BasicForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditProjectBasic($scope.BasicForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalReward, $scope.RewardPKgs)) {
            if ($scope.rewardForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditReward($scope.rewardForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalUpdateLog, $scope.UpdateLogs)) {
            if ($scope.updateLogForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditUpdateLog($scope.updateLogForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalStory, $scope.ProjectStory)) {
            if ($scope.storyForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditStory($scope.storyForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalQuestion, $scope.Question)) {
            if ($scope.questionForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditQuestion($scope.questionForm);
            }
            e.stopImmediatePropagation();
        }

        if (!angular.equals($scope.originalTimeline, $scope.Timeline)) {
            if ($scope.timelineForm.$invalid) {
                $scope.checkForm();
            } else {
                $scope.checkEditTimeline($scope.timelineForm);
            }
            e.stopImmediatePropagation();
        }

        // Get the tab want to move to
        $scope.thisTab = $(this);
    });

    // If form is dirty
    $scope.checkForm = function () {
        swal({ title: "Lỗi!", text: "Sai thông tin. Xin hãy xem lại!", type: "warning" });
    };

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
                    $scope.selectedCate();
                    $scope.selectedOption = angular.copy($scope.originalSelectedCate);
                    form.$setPristine();
                    form.$setUntouched();
                    $(".projecImg-preview").attr("src", $scope.Project.ImageUrl);
                    //reset file field
                    var resetImg = $("#imgSelected");
                    resetImg.replaceWith(resetImg = resetImg.clone(true));
                    $scope.file = null;
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
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
                    $scope.changeTab($scope.thisTab.context.hash);
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
                } else {
                    SweetAlert.swal("Cancelled", "Project's update log is safe :)", "error");
                    $scope.UpdateLogs = angular.copy($scope.originalUpdateLog);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
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
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab story is dirty
    $scope.checkEditQuestion = function (form) {
        SweetAlert.swal({
            title: "You have changed something!",
            text: "Project's Q&A will be edit!",
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
                    SweetAlert.swal("Edited!", "Project's Q&A has been edited.", "success");
                    $scope.updateQuestion(form);
                } else {
                    SweetAlert.swal("Cancelled", "Project's Q&A is safe :)", "error");
                    $scope.Question = angular.copy($scope.originalQuestion);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // If tab timeline is dirty
    $scope.checkEditTimeline = function (form) {
        SweetAlert.swal({
            title: "You have changed something!",
            text: "Project's timeline will be edit!",
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
                    SweetAlert.swal("Edited!", "Project's timeline has been edited.", "success");
                    for (var i = 0; i < $scope.Timeline.length; i++) {
                        if ($scope.Timeline[i] !== $scope.originalTimeline[i]) {
                            $scope.updateTimeline(i);
                            break;
                        };
                    }
                } else {
                    SweetAlert.swal("Cancelled", "Project's timeline is safe :)", "error");
                    $scope.Timeline = angular.copy($scope.originalTimeline);
                    form.$setPristine();
                    form.$setUntouched();
                    // Switch tab
                    $scope.changeTab($scope.thisTab.context.hash);
                }
            });
    };

    // Change tab
    $scope.changeTab = function (toTab) {
        if (toTab === "#reward") {
            $scope.getReward();
        }

        if (toTab === "#story") {
            $scope.getStory();
        }

        if (toTab === "#update") {
            $scope.getUpdateLog();
        }

        if (toTab === "#quesion-answer") {
            $scope.getQuestion();
        }

        if (toTab === "#timelineTab") {
            $scope.getTimeline();
        }

        $scope.thisTab.tab('show');
    };

    // Discard project basic
    $scope.resetProjectBasic = function (form) {
        if (form) {
            $scope.Project = angular.copy($scope.originalProjectBasic);
            $scope.selectedCate();
            $scope.selectedOption = angular.copy($scope.originalSelectedCate);
            form.$setPristine();
            form.$setUntouched();
            $(".projecImg-preview").attr("src", $scope.Project.ImageUrl);
            //reset file field
            var resetImg = $("#imgSelected");
            resetImg.replaceWith(resetImg = resetImg.clone(true));
            $scope.file = null;
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
    // Discard question
    $scope.resetQuestion = function (form) {
        if (form) {
            $scope.Question = angular.copy($scope.originalQuestion);
            form.$setPristine();
            form.$setUntouched();
        }
    };

    $scope.turnEditTimeline = function (index) {
        $scope.editTimeline[index] = true;
        $("#title" + index).prop('disabled', false);
        $("#date" + index).prop('disabled', false);
        $("#" + index).prop('disabled', false);
        $("#desc" + index).prop('disabled', false);
    };

    $scope.onNewPoint = function () {
        $scope.isCreateTimeline = true;
    };

    $scope.offNewPoint = function () {
        $scope.isCreateTimeline = false;
    };

    // submit project
    $scope.submit = function () {
        $scope.dirtyForm();
        // Put update project
        var promisePut = ProjectService.submitProject($scope.Project);

        promisePut.then(
            function (result) {
                if (result.data.Status === "success") {
                    toastr.success('Submit thành công!', 'Thành công!');
                    $scope.Project = result.data.Data;
                    $scope.errorListFlag = false;
                    $scope.AllEditable = false;
                    $scope.originalProjectBasic = angular.copy($scope.Project);

                } else {
                    $scope.errorListFlag = true;
                    $scope.errorList = result.data.Data;
                    console.log("error" + $scope.errorList);
                    toastr.error($scope.Error, 'Lỗi!');
                }
            },
            function (error) {
                toastr.error($scope.Error, 'Lỗi!');
            });
    }
});