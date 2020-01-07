"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var aurelia_framework_1 = require("aurelia-framework");
var aurelia_router_1 = require("aurelia-router");
var aurelia_http_client_1 = require("aurelia-http-client");
var JobDetailsView = (function () {
    function JobDetailsView(router, http) {
        this.router = router;
        this.http = http;
    }
    JobDetailsView.prototype.activate = function (params) {
        this.schedulerName = params.schedulerName;
        this.group = params.group;
        this.name = params.name;
        return this.loadDetails();
    };
    JobDetailsView.prototype.pause = function () {
        this.postCommand("pause");
    };
    JobDetailsView.prototype.resume = function () {
        this.postCommand("resume");
    };
    JobDetailsView.prototype.trigger = function () {
        this.postCommand("trigger");
    };
    JobDetailsView.prototype.delete = function () {
        var _this = this;
        bootbox.confirm({
            size: "small",
            message: "Delete " + this.name + "?",
            callback: function (result) {
                if (result) {
                    return _this.http.delete("/api/schedulers/" + _this.schedulerName + "/jobs/" + _this.group + "/" + _this.name).then(function () {
                        toastr.success("Job " + _this.group + "." + _this.name + " deleted successfully");
                        return _this.router.navigate("jobs");
                    });
                }
                return $.when();
            }
        });
    };
    JobDetailsView.prototype.postCommand = function (command) {
        var _this = this;
        return this.http.post("/schedulers/" + this.schedulerName + "/jobs/" + this.group + "/" + this.name + "/" + command, null).then(function () {
            return _this.loadDetails();
        });
    };
    JobDetailsView.prototype.loadDetails = function () {
        var _this = this;
        return this.http.get("/api/schedulers/" + this.schedulerName + "/jobs/" + this.group + "/" + this.name + "/details").then(function (response) {
            _this.details = response.content;
        });
    };
    var _a, _b;
    JobDetailsView = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [typeof (_a = typeof aurelia_router_1.Router !== "undefined" && aurelia_router_1.Router) === "function" ? _a : Object, typeof (_b = typeof aurelia_http_client_1.HttpClient !== "undefined" && aurelia_http_client_1.HttpClient) === "function" ? _b : Object])
    ], JobDetailsView);
    return JobDetailsView;
}());
exports.JobDetailsView = JobDetailsView;
//# sourceMappingURL=job-details.js.map