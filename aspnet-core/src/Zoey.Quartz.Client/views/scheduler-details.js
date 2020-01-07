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
var SchedulerIndexView = (function () {
    function SchedulerIndexView(router, http) {
        this.router = router;
        this.http = http;
        this.loadingCurrentlyExecutingJobs = false;
    }
    SchedulerIndexView.prototype.currentlyExistingJobsExist = function () {
        return this.currentlyExecutingJobs && this.currentlyExecutingJobs.length > 0;
    };
    SchedulerIndexView.prototype.activate = function (params) {
        this.schedulerName = params.schedulerName;
        return this.loadDetails();
    };
    SchedulerIndexView.prototype.standby = function () {
        this.postCommand("standby");
    };
    SchedulerIndexView.prototype.start = function () {
        this.postCommand("start");
    };
    SchedulerIndexView.prototype.shutdown = function () {
        this.postCommand("shutdown");
    };
    SchedulerIndexView.prototype.postCommand = function (command) {
        var _this = this;
        return this.http.post("/api/schedulers/" + this.schedulerName + "/" + command, null).then(function () {
            return _this.loadDetails();
        });
    };
    SchedulerIndexView.prototype.loadDetails = function () {
        var _this = this;
        return Promise.all([
            this.http.get("/api/schedulers/" + this.schedulerName).then(function (response) {
                _this.details = response.content;
            }),
            this.refreshCurrentlyExecutingJobs()
        ]);
    };
    SchedulerIndexView.prototype.refreshCurrentlyExecutingJobs = function () {
        var _this = this;
        this.loadingCurrentlyExecutingJobs = true;
        return this.http.get("/api/schedulers/" + this.schedulerName + "/jobs/currently-executing")
            .then(function (response) {
            _this.currentlyExecutingJobs = response.content;
        })
            .catch(function () { })
            .then(function () {
            _this.loadingCurrentlyExecutingJobs = false;
        });
    };
    var _a, _b;
    SchedulerIndexView = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [typeof (_a = typeof aurelia_router_1.Router !== "undefined" && aurelia_router_1.Router) === "function" ? _a : Object, typeof (_b = typeof aurelia_http_client_1.HttpClient !== "undefined" && aurelia_http_client_1.HttpClient) === "function" ? _b : Object])
    ], SchedulerIndexView);
    return SchedulerIndexView;
}());
exports.SchedulerIndexView = SchedulerIndexView;
//# sourceMappingURL=scheduler-details.js.map