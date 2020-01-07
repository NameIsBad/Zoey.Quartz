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
var LiveLogsView = (function () {
    function LiveLogsView(router, http) {
        var _this = this;
        this.router = router;
        this.http = http;
        this.entries = [];
        this.numberOfEntriesToShow = "50";
        this.showJobInfo = true;
        this.showTriggerInfo = true;
        this.liveLogHub = $.connection.liveLogHub;
        this.liveLogHub
            .on("triggerFired", function (trigger) {
            if (_this.showTriggerInfo)
                _this.showMessage("Trigger <strong>" + trigger.Name + "." + trigger.Group + "</strong> fired");
        })
            .on("triggerMisfired", function (trigger) {
            if (_this.showTriggerInfo)
                _this.showMessage("<strong>Trigger " + trigger.Name + "." + trigger.Group + " misfired</strong>");
        })
            .on("triggerCompleted", function (trigger) {
            if (_this.showTriggerInfo)
                _this.showMessage("Trigger <strong>" + trigger.Name + "." + trigger.Group + "</strong> has completed");
        })
            .on("triggerPaused", function (triggerKey) {
            if (_this.showTriggerInfo)
                _this.showMessage("Trigger <strong>" + triggerKey.Name + "." + triggerKey.Group + "</strong> was paused");
        })
            .on("triggerResumed", function (triggerKey) {
            if (_this.showTriggerInfo)
                _this.showMessage("Trigger <strong>" + triggerKey.Name + "." + triggerKey.Group + "</strong> was resumed");
        })
            .on("jobPaused", function (jobKey) {
            if (_this.showJobInfo)
                _this.showMessage("Job <strong>" + jobKey.Name + "." + jobKey.Group + "</strong> was paused");
        })
            .on("jobResumed", function (jobKey) {
            if (_this.showJobInfo)
                _this.showMessage("Job <strong>" + jobKey.Name + "." + jobKey.Group + "</strong> was resumed");
        })
            .on("jobToBeExecuted", function (jobKey, triggerKey) {
            if (_this.showJobInfo)
                _this.showMessage("Starting to execute job <strong>" + jobKey.Name + "." + jobKey.Group + "</strong> triggered by trigger <strong>" + triggerKey.Name + "." + triggerKey.Group + "</strong>...");
        })
            .on("jobWasExecuted", function (jobKey, triggerKey, errorMessage) {
            if (_this.showJobInfo) {
                var message = "Job <strong>" + jobKey.Name + "." + jobKey.Group + "</strong> was executed";
                if (errorMessage) {
                    message += " and ended with error: " + errorMessage;
                }
                _this.showMessage(message);
            }
        });
        $.connection.hub.error(function (error) {
            _this.showMessage(error);
        });
    }
    LiveLogsView.prototype.attached = function () {
        var _this = this;
        this.showMessage("Connecting to hub...");
        $.connection.hub.start()
            .then(function () {
            _this.showMessage("Connected");
        })
            .fail(function () {
            _this.showMessage("SignalR error: Could not start hub connection");
        });
    };
    LiveLogsView.prototype.showMessage = function (message) {
        while (this.entries.length >= parseInt(this.numberOfEntriesToShow)) {
            this.entries.shift();
        }
        var value = {
            date: moment(),
            message: message
        };
        this.entries.push(value);
    };
    LiveLogsView.prototype.deactivate = function () {
        if (this.liveLogHub && this.liveLogHub.connection) {
            this.liveLogHub.connection.stop();
        }
    };
    var _a, _b;
    LiveLogsView = __decorate([
        aurelia_framework_1.autoinject,
        __metadata("design:paramtypes", [typeof (_a = typeof aurelia_router_1.Router !== "undefined" && aurelia_router_1.Router) === "function" ? _a : Object, typeof (_b = typeof aurelia_http_client_1.HttpClient !== "undefined" && aurelia_http_client_1.HttpClient) === "function" ? _b : Object])
    ], LiveLogsView);
    return LiveLogsView;
}());
exports.LiveLogsView = LiveLogsView;
//# sourceMappingURL=live-logs.js.map