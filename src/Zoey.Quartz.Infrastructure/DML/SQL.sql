
-- ----------------------------
-- Table structure for Log
-- ----------------------------
DROP TABLE IF EXISTS "Log";
CREATE TABLE "Log" (
  "ExecutionTime" text(20) NOT NULL,
  "ClientIpAddress" TEXT(50),
  "BrowserInfo" TEXT(300),
  "UserName" TEXT(100),
  "Content" TEXT(2000)
);


-- ----------------------------
-- Table structure for TaskLog
-- ----------------------------
DROP TABLE IF EXISTS "TaskLog";
CREATE TABLE "TaskLog" (
  "Id" integer NOT NULL PRIMARY KEY AUTOINCREMENT,
  "TaskId" INTEGER NOT NULL,
  "ExecutionTime" TEXT(50) NOT NULL,
  "ExecutionDuration" integer NOT NULL,
  "Msg" TEXT(2000),
  "TaskLogStatus" integer NOT NULL
);


-- ----------------------------
-- Table structure for TaskOptions
-- ----------------------------
DROP TABLE IF EXISTS "TaskOptions";
CREATE TABLE "TaskOptions" (
  "Id" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
  "TaskName" TEXT(120) NOT NULL,
  "GroupName" TEXT(120) NOT NULL,
  "Interval" TEXT(120) NOT NULL,
  "ApiUrl" TEXT(300),
  "AuthKey" TEXT(300),
  "AuthValue" TEXT(300),
  "Describe" TEXT(500),
  "RequestType" integer NOT NULL,
  "LastRunTime" TEXT(50),
  "Status" integer
);