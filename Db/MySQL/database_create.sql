-- 创建数据库
CREATE DATABASE `yus_x_dev`;

USE `yus_x_dev`;

-- 允许从本地文件读取
SET GLOBAL local_infile = 'ON';
-- 设置字符集威UTF8MB4
SET NAMES utf8mb4;
-- 关闭外键检查
SET FOREIGN_KEY_CHECKS = 0;

-- 省份(Sys_Province)
-- DROP TABLE IF EXISTS `Sys_Province`;
CREATE TABLE IF NOT EXISTS `Sys_Province`
(
    `ProvinceCode` int PRIMARY KEY NOT NULL COMMENT '省份代码',
    `ProvinceName` varchar(30) NOT NULL COMMENT '省份名称',
    `RegionCode` varchar(10) DEFAULT NULL COMMENT '区域代码'
) COMMENT '省份';

-- 城市(Sys_City)
-- DROP TABLE IF EXISTS `Sys_City`;
CREATE TABLE IF NOT EXISTS `Sys_City`
(
    `CityCode` int PRIMARY KEY NOT NULL COMMENT '城市代码',
    `CityName` varchar(30) DEFAULT NULL COMMENT '城市名称',
    `ProvinceCode` int NOT NULL COMMENT '省份代码'
) COMMENT '城市';

-- 区县(Sys_County)
-- DROP TABLE IF EXISTS `Sys_County`;
CREATE TABLE IF NOT EXISTS `Sys_County`
(
    `CountyCode` int PRIMARY KEY NOT NULL COMMENT '区县代码',
    `CountyName` varchar(30) DEFAULT NULL COMMENT '区县名称',
    `CityCode` int NOT NULL COMMENT '城市代码',
    `ProvinceCode` int NOT NULL COMMENT '省份代码'
) COMMENT '区县';

-- 字典(Sys_Dictionary)
-- DROP TABLE IF EXISTS `Sys_Dictionary`;
CREATE TABLE IF NOT EXISTS `Sys_Dictionary`
(
    `DictId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '字典ID',
    `DictNo` varchar(100) NOT NULL COMMENT '字典编号',
    `Name` varchar(100) NOT NULL COMMENT '字典名称',
    `ParentId` int NOT NULL DEFAULT 0 COMMENT '父级ID',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `Remark` varchar(4000) DEFAULT NULL COMMENT '备注',
    `DbSql` text DEFAULT NULL COMMENT 'SQL语句',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '字典';

INSERT INTO `Sys_Dictionary` (DictId, DictNo, Name, ParentId, Enable, SortNo, Remark, DbSql, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, 'CommonHas', '有无值', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, 'CommonYesNo', '是否值', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (3, 'CommonEnable', '启用状态', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (4, 'CommonGender', '性别', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (5, 'CommonGenderSecret', '性别(含保密)', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (6, 'CommonExpire', '过期状态', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (7, 'CommonAuditStatus', '审核状态', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (8, 'CommonRoleList', '角色列表', 0, 1, 0, NULL, 'SELECT RoleId as `key`, RoleName as `value` FROM Sys_Role WHERE Enable = 1', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (9, 'CommonRoleTree', '级联角色', 0, 1, 0, NULL, 'SELECT RoleId AS `id`, parentId, RoleId AS `key`, RoleName AS `value` FROM Sys_Role WHERE Enable = 1', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (10, 'CommonProvinceList', '省份列表', 0, 1, 0, NULL, 'SELECT ProvinceCode AS `key`, ProvinceName AS `value` FROM Sys_Province', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (11, 'CommonCityList', '城市列表', 0, 1, 0, NULL, 'SELECT CityCode AS `key`, CityName AS `value` FROM Sys_City', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (12, 'CommonCountyList', '区县列表', 0, 1, 0, NULL, 'SELECT CountyCode AS `key`, CountyName AS `value` FROM Sys_County', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (100, 'SysLogLogType', '日志类型', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (101, 'SysLogLevel', '日志级别', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (102, 'SysApiLogResponseStatus', '接口日志响应状态', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, 'SysApiLogType', '接口日志类型', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (104, 'SysUserLogType', '用户日志类型', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (105, 'SysTokenType', '令牌类型', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (106, 'SysTokenCodeType', '令牌代码类型', 0, 1, 0, NULL, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 字典项(Sys_DictionaryItem)
-- DROP TABLE IF EXISTS `Sys_DictionaryItem`;
CREATE TABLE IF NOT EXISTS `Sys_DictionaryItem`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `DictId` int NOT NULL COMMENT '字典ID',
    `Name` varchar(100) NOT NULL COMMENT '字典项名称',
    `Value` varchar(100) NOT NULL COMMENT '字典项值',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int DEFAULT NULL DEFAULT 0 COMMENT '排序号',
    `Remark` varchar(2000) DEFAULT NULL COMMENT '备注',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '字典明细';

INSERT INTO Sys_DictionaryItem (DictId, Name, Value, Enable, SortNo, Remark, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, '无', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (1, '有', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 是否值
       (2, '否', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, '是', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 启用状态
       (3, '禁用', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (3, '启用', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 性别
       (4, '男', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (4, '女', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 性别(含保密)
       (5, '男', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (5, '女', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (5, '保密', '2', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 过期状态
       (6, '已过期', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (6, '未过期', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 审核状态
       (7, '待审核', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (7, '已审核', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (7, '未通过', '2', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 日志类型
       (100, '其他', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (100, '服务', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (100, '数据库', '2', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (100, '系统接口', '3', 1, 85, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (100, '应用接口', '4', 1, 80, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 日志级别
       (101, '冗余', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (101, '调试', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (101, '信息', '2', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (101, '警告', '3', 1, 85, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (101, '错误', '4', 1, 80, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (101, '致命', '5', 1, 75, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 接口日志响应状态
       (102, '其他', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (102, '成功', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (102, '异常', '2', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (102, '信息', '3', 1, 85, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 接口日志类型
       (103, '其他', '0', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '系统', '1', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '登录', '2', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '新增', '3', 1, 85, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '编辑', '4', 1, 80, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '删除', '5', 1, 75, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '修改密码', '6', 1, 70, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '刷新Token', '7', 1, 65, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, 'Token过期', '8', 1, 60, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '应用接口', '9', 1, 55, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '系统接口异常', '10', 1, 50, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (103, '应用接口异常', '11', 1, 45, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 用户日志类型
       (104, '登录', '1', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (104, '修改密码', '2', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 令牌类型
       (105, '用户', '1', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (105, '开发', '2', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (105, '任务', '3', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       -- 令牌代码类型
       (106, 'ID', '1', 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (106, '用户名', '2', 1, 95, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (106, '邮箱', '3', 1, 90, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (106, '手机号', '4', 1, 85, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 标志(Sys_Flag)
-- DROP TABLE IF EXISTS `Sys_Flag`;
CREATE TABLE IF NOT EXISTS `Sys_Flag`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `Flag` varchar(50) NOT NULL COMMENT '标志',
    `Value` text DEFAULT NULL COMMENT '内容',
    `Remark` varchar(200) DEFAULT NULL COMMENT '备注',
    `GroupName` varchar(30) DEFAULT NULL COMMENT '分组',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '标志';

-- 菜单(Sys_Menu)
-- DROP TABLE IF EXISTS `Sys_Menu`;
CREATE TABLE IF NOT EXISTS `Sys_Menu`
(
    `MenuId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '菜单ID',
    `Name` varchar(50) NOT NULL COMMENT '菜单名称',
    `ParentId` int NOT NULL DEFAULT 0 COMMENT '父级ID',
    `Icon` varchar(50) DEFAULT NULL COMMENT '图标',
    `Description` varchar(200) DEFAULT NULL COMMENT '描述',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `TableName` varchar(200) DEFAULT NULL COMMENT '数据表名称',
    `Url` varchar(4000) DEFAULT NULL COMMENT '页面地址',
    `Auth` text DEFAULT NULL COMMENT '授权信息',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '菜单';

INSERT INTO `Sys_Menu` (MenuId, Name, ParentId, Icon, Description, Enable, SortNo, TableName, Url, Auth, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, '开发中心', 0, 'ivu-icon ivu-icon-md-construct', NULL, 1, 3000, NULL, '/Coding', '[{\"text\":\"查询\",\"value\":\"Search\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (100, '代码生成', 1, 'ivu-icon ivu-icon-ios-hammer', NULL, 1, 2990, NULL, '/Coder', '[{\"text\":\"查询\",\"value\":\"Search\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, '用户权限', 0, 'ivu-icon ivu-icon-ios-people', NULL, 1, 2000, NULL, '', '[{\"text\":\"查询\",\"value\":\"Search\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (200, '用户管理', 2, 'ivu-icon ivu-icon-ios-person', NULL, 1, 1990, 'Sys_User', '/SysUser', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"新建\",\"value\":\"Add\"},{\"text\":\"删除\",\"value\":\"Delete\"},{\"text\":\"编辑\",\"value\":\"Update\"},{\"text\":\"导入\",\"value\":\"Import\"},{\"text\":\"导出\",\"value\":\"Export\"},{\"text\":\"上传\",\"value\":\"Upload\"},{\"text\":\"审核\",\"value\":\"Audit\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (201, '角色管理', 2, 'ivu-icon ivu-icon-ios-contacts', NULL, 1, 1990, 'Sys_Role', '/SysRole', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"新建\",\"value\":\"Add\"},{\"text\":\"删除\",\"value\":\"Delete\"},{\"text\":\"编辑\",\"value\":\"Update\"},{\"text\":\"导出\",\"value\":\"Export\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (202, '权限管理', 2, 'ivu-icon ivu-icon-ios-boat', NULL, 1, 1980, 'Sys_Role2', '/Permission', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"编辑\",\"value\":\"Update\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (3, '系统管理', 0, 'ivu-icon ivu-icon-md-settings', NULL, 1, 1000, NULL, '', '[{\"text\":\"查询\",\"value\":\"Search\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (300, '菜单设置', 3, 'ivu-icon ivu-icon-ios-menu', NULL, 1, 990, 'Sys_Menu', '/SysMenu', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"新建\",\"value\":\"Add\"},{\"text\":\"编辑\",\"value\":\"Update\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (301, '字典维护', 3, 'ivu-icon ivu-icon-ios-book', NULL, 1, 980, 'Sys_Dictionary', '/SysDictionary', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"新建\",\"value\":\"Add\"},{\"text\":\"删除\",\"value\":\"Delete\"},{\"text\":\"编辑\",\"value\":\"Update\"},{\"text\":\"导出\",\"value\":\"Export\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (302, '接口日志', 3, 'ivu-icon ivu-icon-ios-magnet', NULL, 1, 970, 'Sys_ApiLog', '/SysApiLog', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"删除\",\"value\":\"Delete\"},{\"text\":\"导出\",\"value\":\"Export\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (303, '系统日志', 3, 'ivu-icon ivu-icon-ios-paper', NULL, 1, 960, 'Sys_Log', '/SysLog', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"删除\",\"value\":\"Delete\"},{\"text\":\"导出\",\"value\":\"Export\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (304, '标志管理', 3, 'ivu-icon ivu-icon-ios-flag', NULL, 1, 950, 'Sys_Flag', '/SysFlag', '[{\"text\":\"查询\",\"value\":\"Search\"},{\"text\":\"新建\",\"value\":\"Add\"},{\"text\":\"删除\",\"value\":\"Delete\"},{\"text\":\"编辑\",\"value\":\"Update\"},{\"text\":\"导出\",\"value\":\"Export\"}]', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 表格(Sys_TableInfo)
-- DROP TABLE IF EXISTS `Sys_TableInfo`;
CREATE TABLE IF NOT EXISTS `Sys_TableInfo`
(
    `TableId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '表格ID',
    `Name` varchar(50) NOT NULL COMMENT '表格名称',
    `ParentId` int NOT NULL DEFAULT 0 COMMENT '父级ID',
    `TableName` varchar(100) DEFAULT NULL COMMENT '数据表名',
    `TableAlias` varchar(100) DEFAULT NULL COMMENT '表格别名',
    `DetailName` varchar(200) DEFAULT NULL COMMENT '明细名称',
    `DetailTable` varchar(200) DEFAULT NULL COMMENT '明细表名',
    `ViewFolder` varchar(200) NOT NULL COMMENT '视图文件夹',
    `ProjectName` varchar(200) NOT NULL COMMENT '项目名称',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `SortField` varchar(50) DEFAULT NULL COMMENT '排序字段',
    `EditField` varchar(50) DEFAULT NULL COMMENT '编辑字段',
    `UploadField` varchar(50) DEFAULT NULL COMMENT '上传字段',
    `UploadMaxCount` int NOT NULL DEFAULT 0 COMMENT '最大上传数'
) COMMENT '表格';

INSERT INTO `Sys_TableInfo` (TableId, Name, ParentId, TableName, TableAlias, DetailName, DetailTable, ViewFolder, ProjectName, Enable, SortNo, SortField, EditField, UploadField, UploadMaxCount)
VALUES (1, '用户权限', 0, NULL, NULL, NULL, NULL, 'System', 'Yus.System', 1, 2000, NULL, NULL, NULL, 0),
       (100, '用户管理', 1, 'Sys_User', NULL, NULL, NULL, 'System', 'Yus.System', 1, 1990, NULL, 'Username', 'HeadImageUrl', 1),
       (101, '角色管理', 1, 'Sys_Role', NULL, NULL, NULL, 'System', 'Yus.System', 1, 1980, NULL, 'Name', NULL, 0),
       (2, '系统管理', 0, NULL, NULL, NULL, NULL, 'System', 'Yus.System', 1, 1000, NULL, NULL, NULL, 0),
       (200, '字典数据', 2, 'Sys_Dictionary', NULL, '字典明细', 'Sys_DictionaryList', 'System', 'Yus.System', 1, 990, NULL, 'Name', NULL, 0),
       (201, '字典明细', 2, 'Sys_DictionaryList', NULL, NULL, NULL, 'System', 'Yus.System', 1, 980, NULL, 'Name', NULL, 0),
       (202, '接口日志', 2, 'Sys_ApiLog', NULL, NULL, NULL, 'System', 'Yus.System', 1, 970, 'Id', NULL, NULL, 0),
       (203, '系统日志', 2, 'Sys_Log', NULL, NULL, NULL, 'System', 'Yus.System', 1, 960, 'Id', NULL, NULL, 0),
       (204, '标志管理', 2, 'Sys_Flag', NULL, NULL, NULL, 'System', 'Yus.System', 1, 950, NULL, 'Flag', NULL, 0);

-- 表格列(Sys_TableColumn)
-- DROP TABLE IF EXISTS `Sys_TableColumn`;
CREATE TABLE IF NOT EXISTS `Sys_TableColumn`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `TableId` int NOT NULL COMMENT '表格ID',
    `Name` varchar(100) NOT NULL COMMENT '显示名称',
    `Field` varchar(100) NOT NULL COMMENT '数据字段',
    `DataType` text NOT NULL COMMENT '数据类型',
    `Width` int NOT NULL COMMENT '显示宽度',
    `ColSize` int DEFAULT NULL COMMENT '显示尺寸',
    `DataSource` varchar(50) DEFAULT NULL COMMENT '选择器数据源',
    `IsDataColumn` tinyint NOT NULL COMMENT '是否数据列',
    `IsShow` tinyint NOT NULL DEFAULT 1 COMMENT '是否显示',
    `IsImage` tinyint NOT NULL COMMENT '是否显示为图片',
    `IsKey` tinyint NOT NULL COMMENT '是否为主键',
    `IsReadonly` tinyint NOT NULL COMMENT '是否只读',
    `Nullable` tinyint NOT NULL COMMENT '是否可空',
    `Sortable` tinyint NOT NULL COMMENT '是否可排序',
    `EditColNo` int DEFAULT NULL COMMENT '编辑列号',
    `EditRowNo` int DEFAULT NULL COMMENT '编辑行号',
    `EditType` varchar(20) DEFAULT NULL COMMENT '编辑类型',
    `SearchColNo` int DEFAULT NULL COMMENT '搜索列号',
    `SearchRowNo` int DEFAULT NULL COMMENT '搜索行号',
    `SearchType` varchar(20) DEFAULT NULL COMMENT '搜索类型',
    `MaxLength` int DEFAULT NULL COMMENT '数据最大长度',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '表格列';

-- 部门(Sys_Department)
-- DROP TABLE IF EXISTS `Sys_Department`;
CREATE TABLE IF NOT EXISTS `Sys_Department`
(
    `DeptId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '部门ID',
    `Name` varchar(20) NOT NULL COMMENT '部门名称',
    `ParentId` int NOT NULL DEFAULT 0 COMMENT '父级ID',
    `AvatarUrl` varchar(50) DEFAULT NULL COMMENT '头像地址',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `Remark` varchar(200) DEFAULT NULL COMMENT '备注',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '部门';

INSERT INTO `Sys_Department` (DeptId, Name, ParentId, AvatarUrl, Enable, SortNo, Remark, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, '管理部', 0, NULL, 1, 1000, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, '财务部', 0, NULL, 1, 950, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (3, '研发部', 0, NULL, 1, 900, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (4, '销售部', 0, NULL, 1, 850, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (5, '服务部', 0, NULL, 1, 800, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 部门授权(Sys_DepartmentAuth)
-- DROP TABLE IF EXISTS `Sys_DepartmentAuth`;
CREATE TABLE IF NOT EXISTS `Sys_DepartmentAuth`
(
    `AuthId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '授权ID',
    `Value` text NOT NULL COMMENT '授权值',
    `MenuId` int NOT NULL COMMENT '菜单ID',
    `DeptId` int NOT NULL COMMENT '部门ID',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '部门授权';

-- 用户(Sys_User)
-- DROP TABLE IF EXISTS `Sys_User`;
CREATE TABLE IF NOT EXISTS `Sys_User`
(
    `UserId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '用户ID',
    `Username` varchar(20) NOT NULL COMMENT '用户名',
    `Nickname` varchar(20) NOT NULL COMMENT '昵称',
    `DeptId` int DEFAULT NULL COMMENT '部门ID',
    `DeptName` varchar(20) DEFAULT NULL COMMENT '部门名称',
    `Gender` tinyint DEFAULT NULL COMMENT '性别',
    `Address` varchar(200) DEFAULT NULL COMMENT '地址',
    `Email` varchar(100) DEFAULT NULL COMMENT '邮箱',
    `RealName` varchar(20) DEFAULT NULL COMMENT '真实名称',
    `Telephone` varchar(20) DEFAULT NULL COMMENT '电话',
    `Mobile` varchar(20) DEFAULT NULL COMMENT '手机号',
    `Fax` varchar(20) DEFAULT NULL COMMENT '传真',
    `AvatarUrl` varchar(200) DEFAULT NULL COMMENT '头像地址',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `Remark` varchar(200) DEFAULT NULL COMMENT '备注',
    `Password` char(32) NOT NULL COMMENT '密码',
    `Salt` char(32) NOT NULL COMMENT '加盐值',
    `AuditStatus` tinyint NOT NULL COMMENT '审核状态',
    `AuditId` int DEFAULT NULL COMMENT '审核人ID',
    `Auditor` varchar(30) DEFAULT NULL COMMENT '审核人',
    `AuditDate` datetime DEFAULT NULL COMMENT '审核时间',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '用户';

INSERT INTO `Sys_User` (UserId, Username, Nickname, DeptId, DeptName, Gender, Address, Email, RealName, Telephone, Mobile, Fax, AvatarUrl, Enable, SortNo, Remark, Password, Salt, AuditStatus, AuditId, Auditor, AuditDate, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, 'admin', '超级管理员', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, NULL, 'DB7A7770B8A653BF9C6953E9927BD0EF', '5E9D4F4711D84682B33DA43C55E4A94E', 1, 1, '超级管理员', '2021-01-01 00:00:00', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, 'test', '测试用户', 0, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 1, 0, NULL, 'BF7D16B928EE104C200855F60D980FF5', '3195EB1788374D078A3317FB3C15B3AB', 1, 1, '超级管理员', '2021-01-01 00:00:00', 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 令牌(Sys_Token)
-- DROP TABLE IF EXISTS `Sys_Token`;
CREATE TABLE IF NOT EXISTS `Sys_Token`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `Type` int NOT NULL COMMENT '类型',
    `UserCode` varchar(20) DEFAULT NULL COMMENT '使用者代码',
    `CodeType` int NOT NULL DEFAULT 1 COMMENT '代码类型',
    `UseTimes` int NOT NULL DEFAULT 0 COMMENT '使用次数',
    `ActiveDate` datetime DEFAULT NULL COMMENT '生效时间',
    `ExpireDate` datetime DEFAULT NULL COMMENT '过期时间',
    `Token` varchar(500) NOT NULL COMMENT '令牌值',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '令牌';

-- 角色(Sys_Role)
-- DROP TABLE IF EXISTS `Sys_Role`;
CREATE TABLE IF NOT EXISTS `Sys_Role`
(
    `RoleId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '角色ID',
    `Name` varchar(50) NOT NULL COMMENT '名称',
    `ParentId` int NOT NULL DEFAULT 0 COMMENT '父级ID',
    `Enable` tinyint NOT NULL DEFAULT 1 COMMENT '是否启用',
    `SortNo` int NOT NULL DEFAULT 0 COMMENT '排序号',
    `Remark` varchar(200) DEFAULT NULL COMMENT '备注',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '角色';

INSERT INTO `Sys_Role` (RoleId, Name, ParentId, Enable, SortNo, Remark, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, '超级管理员', 0, 1, 50, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, '系统管理员', 0, 1, 100, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (3, '开发者', 0, 1, 150, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (4, '测试员', 0, 1, 200, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (5, '管理人员', 0, 1, 300, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (6, '工作人员', 0, 1, 250, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (7, '系统演示', 0, 1, 0, NULL, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 角色授权(Sys_RoleAuth)
-- DROP TABLE IF EXISTS `Sys_RoleAuth`;
CREATE TABLE IF NOT EXISTS `Sys_RoleAuth`
(
    `AuthId` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT '授权ID',
    `Value` text NOT NULL COMMENT '授权值',
    `MenuId` int NOT NULL COMMENT '菜单ID',
    `RoleId` int NOT NULL COMMENT '角色ID',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '角色授权';

-- 用户角色(Sys_UserRole)
-- DROP TABLE IF EXISTS `Sys_UserRole`;
CREATE TABLE IF NOT EXISTS `Sys_UserRole`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `UserId` int NOT NULL COMMENT '用户ID',
    `RoleId` int NOT NULL COMMENT '角色ID',
    `CreateId` int NOT NULL COMMENT '创建人ID',
    `Creator` varchar(30) NOT NULL COMMENT '创建人',
    `CreateDate` datetime NOT NULL COMMENT '创建时间',
    `ModifyId` int DEFAULT NULL COMMENT '修改人ID',
    `Modifier` varchar(30) DEFAULT NULL COMMENT '修改人',
    `ModifyDate` datetime DEFAULT NULL COMMENT '修改时间'
) COMMENT '用户角色';

INSERT INTO `Sys_UserRole` (Id, UserId, RoleId, CreateId, Creator, CreateDate, ModifyId, Modifier, ModifyDate)
VALUES (1, 1, 1, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL),
       (2, 2, 4, 1, '超级管理员', '2021-01-01 00:00:00', NULL, NULL, NULL);

-- 日志(Sys_Log)
-- DROP TABLE IF EXISTS `Sys_Log`;
CREATE TABLE IF NOT EXISTS `Sys_Log`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `Content` text NOT NULL COMMENT '内容',
    `Type` int NOT NULL DEFAULT 0 COMMENT '类型',
    `Level` int NOT NULL DEFAULT 0 COMMENT '级别',
    `ExceptionInfo` text DEFAULT NULL COMMENT '异常信息',
    `RecordDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP() COMMENT '记录时间',
    `UserId` int DEFAULT NULL COMMENT '用户ID',
    `UserName` varchar(30) DEFAULT NULL COMMENT '用户名称'
) COMMENT '日志';

-- 接口日志(Sys_ApiLog)
-- DROP TABLE IF EXISTS `Sys_ApiLog`;
CREATE TABLE IF NOT EXISTS `Sys_ApiLog`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `Url` text DEFAULT NULL COMMENT '请求地址',
    `Request` text DEFAULT NULL COMMENT '请求内容',
    `Response` text DEFAULT NULL COMMENT '响应内容',
    `Type` int NOT NULL DEFAULT 0 COMMENT '类型',
    `BeginDate` datetime DEFAULT NULL COMMENT '开始时间',
    `EndDate` datetime DEFAULT NULL COMMENT '结束时间',
    `ElapsedTime` int DEFAULT NULL COMMENT '时长',
    `ResponseStatus` int NOT NULL DEFAULT 0 COMMENT '响应状态',
    `ExceptionInfo` text DEFAULT NULL COMMENT '异常信息',
    `UserAgent` varchar(500) DEFAULT NULL COMMENT '用户代理',
    `UserAddress` varchar(30) DEFAULT NULL COMMENT '用户IP',
    `UserId` int DEFAULT NULL COMMENT '用户ID',
    `UserName` varchar(30) DEFAULT NULL COMMENT '用户名称'
) COMMENT '接口日志';

-- 用户日志(Sys_UserLog)
-- DROP TABLE IF EXISTS `Sys_UserLog`;
CREATE TABLE IF NOT EXISTS `Sys_UserLog`
(
    `Id` int PRIMARY KEY NOT NULL AUTO_INCREMENT COMMENT 'ID',
    `Type` int NOT NULL COMMENT '类型',
    `Content` text DEFAULT NULL COMMENT '内容',
    `OldValue` varchar(500) DEFAULT NULL COMMENT '原始值',
    `NewValue` varchar(500) DEFAULT NULL COMMENT '修改值',
    `RecordDate` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP() COMMENT '记录时间',
    `UserId` int DEFAULT NULL COMMENT '用户ID',
    `UserName` varchar(30) DEFAULT NULL COMMENT '用户名称'
) COMMENT '用户日志';
