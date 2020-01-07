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
var JobHistoryView = (function () {
    function JobHistoryView(router, http) {
        this.router = router;
        this.http = http;
    }
    JobHistoryView.prototype.activate = function (params) {
        var _this = this;
        return this.http.get("/api/schedulers/" + params.schedulerName + "/jobs/history").then(function (response) {
            var model = response.content;
            _this.entries = model.historyEntries;
            _this.errorMessage = model.errorMessage;
        });
    };
    var _a, _b;
    JobHistoryView = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [typeof (_a = typeof aurelia_router_1.Router !== "undefined" && aurelia_router_1.Router) === "function" ? _a : Object, typeof (_b = typeof aurelia_http_client_1.HttpClient !== "undefined" && aurelia_http_client_1.HttpClient) === "function" ? _b : Object])
    ], JobHistoryView);
    return JobHistoryView;
}());
exports.JobHistoryView = JobHistoryView;
//# sourceMappingURL=history.js.map