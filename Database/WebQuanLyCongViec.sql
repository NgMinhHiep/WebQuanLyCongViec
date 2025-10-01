USE db_Web_QuanLyCongViec

-- Tạo bảng tblRole
CREATE TABLE tblRole (
    iRoleID INT IDENTITY(1,1) PRIMARY KEY,   -- Khóa chính, tự tăng
    sRoleName NVARCHAR(200) NOT NULL         -- Tên vai trò
);

-- Tạo bảng tblUser
CREATE TABLE tblUser (
    iUserID INT IDENTITY(1,1) PRIMARY KEY,      -- Khóa chính
    sFullName NVARCHAR(100) NOT NULL,           -- Họ và tên
    dDateOfBirth DATE NULL,                     -- Ngày sinh
    sPhoneNumber VARCHAR(12) NULL,              -- Số điện thoại
    sEmail VARCHAR(1000) NULL,                  -- Email
    sUserName VARCHAR(70) NOT NULL,             -- Tên tài khoản
    sPassWord NVARCHAR(256) NOT NULL,           -- Mật khẩu
    dUser_CreationDate DATETIME DEFAULT GETDATE(), -- Ngày tạo tài khoản
    iRoleID INT NOT NULL,                       -- Khóa ngoại đến tblRole
    CONSTRAINT FK_tblUser_tblRole FOREIGN KEY (iRoleID)
        REFERENCES tblRole(iRoleID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblNotification
CREATE TABLE tblNotification (
    iNotificationID INT IDENTITY(1,1) PRIMARY KEY,  -- Khóa chính
    iUserID INT NOT NULL,                           -- Khóa ngoại đến tblUser
    sTitle NVARCHAR(200) NOT NULL,                  -- Tiêu đề thông báo
    sMessage NVARCHAR(MAX) NULL,                    -- Nội dung thông báo
    dCreatedTime DATETIME DEFAULT GETDATE(),        -- Thời gian tạo thông báo
    bIsRead BIT DEFAULT 0,                          -- Trạng thái đã đọc (0 = chưa, 1 = đã)
    sType NVARCHAR(50) NULL,                        -- Loại thông báo
    CONSTRAINT FK_tblNotification_tblUser FOREIGN KEY (iUserID)
        REFERENCES tblUser(iUserID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblPersonalTask
CREATE TABLE tblPersonalTask (
    iPersonalTaskID INT IDENTITY(1,1) PRIMARY KEY,   -- Khóa chính
    iUserID INT NOT NULL,                            -- Khóa ngoại đến tblUser
    sPersonalTaskName NVARCHAR(100) NOT NULL,        -- Tên công việc cá nhân
    sPersonalTaskDescription NVARCHAR(1000) NULL,    -- Mô tả công việc cá nhân
    sPersonalTaskStatus NVARCHAR(50) NOT NULL,       -- Trạng thái công việc
    sPriorityLevel NVARCHAR(50) NULL,                -- Độ ưu tiên
    dStartDate DATE NULL,                            -- Ngày bắt đầu
    dEndDate DATE NULL,                              -- Ngày kết thúc
    CONSTRAINT FK_tblPersonalTask_tblUser FOREIGN KEY (iUserID)
        REFERENCES tblUser(iUserID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblPersonalNote
CREATE TABLE tblPersonalNote (
    iPersonalNoteID INT IDENTITY(1,1) PRIMARY KEY,   -- Khóa chính
    iPersonalTaskID INT NOT NULL,                    -- Khóa ngoại đến tblPersonalTask
    sPersonalNoteDetails NVARCHAR(1000) NOT NULL,    -- Nội dung ghi chú
    dPersonalNote_CreationDate DATETIME DEFAULT GETDATE(), -- Ngày tạo ghi chú
    CONSTRAINT FK_tblPersonalNote_tblPersonalTask FOREIGN KEY (iPersonalTaskID)
        REFERENCES tblPersonalTask(iPersonalTaskID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblGroup
CREATE TABLE tblGroup (
    iGroupID INT IDENTITY(1,1) PRIMARY KEY,     -- Khóa chính
    iLeaderID INT NOT NULL,                     -- Khóa ngoại đến tblUser (nhóm trưởng)
    sGroupName NVARCHAR(200) NOT NULL,          -- Tên nhóm
    dGroupFormationDate DATE DEFAULT GETDATE(), -- Ngày tạo nhóm
    CONSTRAINT FK_tblGroup_tblUser FOREIGN KEY (iLeaderID)
        REFERENCES tblUser(iUserID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblGroupMember
CREATE TABLE tblGroupMember (
    iUserID INT NOT NULL,
    iGroupID INT NOT NULL,
    dGroupEntryDate DATETIME DEFAULT GETDATE(),
    CONSTRAINT PK_tblGroupMember PRIMARY KEY (iUserID, iGroupID),
    CONSTRAINT FK_tblGroupMember_tblGroup FOREIGN KEY (iGroupID)
        REFERENCES tblGroup(iGroupID)
        ON DELETE CASCADE
        ON UPDATE CASCADE,
	CONSTRAINT FK_tblGroupMember_tblUser FOREIGN KEY (iUserID)
        REFERENCES tblUser(iUserID)
);

-- Tạo bảng tblParentGroupTask
CREATE TABLE tblParentGroupTask (
    iParentGroupTaskID INT IDENTITY(1,1) PRIMARY KEY,   -- Khóa chính
    iGroupID INT NOT NULL,                              -- Khóa ngoại đến tblGroup
    sParentGroupTaskName NVARCHAR(100) NOT NULL,        -- Tên công việc cha
    sParentGroupTaskDescription NVARCHAR(1000) NULL,    -- Mô tả công việc cha
    sParentGroupTaskStatus NVARCHAR(50) NOT NULL,       -- Trạng thái
    sPriorityLevel NVARCHAR(50) NULL,                   -- Độ ưu tiên
    CONSTRAINT FK_tblParentGroupTask_tblGroup FOREIGN KEY (iGroupID)
        REFERENCES tblGroup(iGroupID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblGroupTaskFile
CREATE TABLE tblGroupTaskFile (
    iGroupTaskFileID INT IDENTITY(1,1) PRIMARY KEY,   -- Khóa chính
    sSenderName NVARCHAR(255) NOT NULL,               -- Tên người gửi
    sFileName NVARCHAR(255) NOT NULL,                 -- Tên file
    sGoogleDriveFileID VARCHAR(255) NOT NULL,         -- Mã file trên Google Drive
    fFileSize FLOAT NULL,                             -- Kích thước file (MB, KB...)
    sFileType VARCHAR(255) NULL,                      -- Loại file (pdf, docx, ...)
    dUploadedTime DATETIME DEFAULT GETDATE(),         -- Thời gian đăng lên
    iParentGroupTaskID INT NOT NULL,                  -- Khóa ngoại đến tblParentGroupTask
    CONSTRAINT FK_tblGroupTaskFile_tblParentGroupTask FOREIGN KEY (iParentGroupTaskID)
        REFERENCES tblParentGroupTask(iParentGroupTaskID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblGroupTask
CREATE TABLE tblGroupTask (
    iGroupTaskID INT PRIMARY KEY IDENTITY(1,1),
    sGroupTaskName NVARCHAR(100) NOT NULL,
    sGroupTaskDescription NVARCHAR(1000),
    sGroupTaskStatus NVARCHAR(50),
    sPriorityLevel NVARCHAR(50),
    dStartDate DATE,
    dEndDate DATE,
    iLateCount INT DEFAULT 0,

    -- Cặp khóa ngoại đến tblGroupMember
    iUserID INT NOT NULL,

    iParentGroupTaskID INT NOT NULL,

	CONSTRAINT FK_tblGroupTask_tblUser FOREIGN KEY (iUserID)
        REFERENCES tblUser(iUserID),
    CONSTRAINT FK_tblGroupTask_tblParentGroupTask FOREIGN KEY (iParentGroupTaskID)
        REFERENCES tblParentGroupTask(iParentGroupTaskID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblGroupNote
CREATE TABLE tblGroupNote (
    iGroupNoteID INT PRIMARY KEY IDENTITY(1,1),
    iGroupTaskID INT NOT NULL,
    sGroupNoteDetails NVARCHAR(1000),
    dGroupNote_CreationDate DATETIME DEFAULT GETDATE(),

    CONSTRAINT FK_tblGroupNote_tblGroupTask FOREIGN KEY (iGroupTaskID)
        REFERENCES tblGroupTask(iGroupTaskID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblReportTaskFile 
CREATE TABLE tblReportTaskFile (
    iReportTaskFileID INT PRIMARY KEY IDENTITY(1,1),
    sReportFileName NVARCHAR(255) NOT NULL,
    sGoogleDriveFileID VARCHAR(255) NOT NULL,
    fFileSize FLOAT,
    sFileType VARCHAR(255),
    dUploadedTime DATETIME DEFAULT GETDATE(),
    iGroupTaskID INT NOT NULL,

    CONSTRAINT FK_tblReportTaskFile_tblGroupTask FOREIGN KEY (iGroupTaskID)
        REFERENCES tblGroupTask(iGroupTaskID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

-- Tạo bảng tblFeedbackTask
CREATE TABLE tblFeedbackTask (
    iFeedbackTaskID INT PRIMARY KEY IDENTITY(1,1),
    sFeedbackDetails NVARCHAR(1000) NOT NULL,
    dFeedbackTime DATETIME DEFAULT GETDATE(),
    iReportTaskFileID INT NOT NULL,

    CONSTRAINT FK_tblFeedbackTask_tblReportTaskFile FOREIGN KEY (iReportTaskFileID)
        REFERENCES tblReportTaskFile(iReportTaskFileID)
        ON DELETE CASCADE
        ON UPDATE CASCADE
);

DROP TABLE tblGroupMember
DROP TABLE tblGroup
DROP TABLE tblParentGroupTask
DROP TABLE tblGroupTaskFile
DROP TABLE tblGroupTask
DROP TABLE tblGroupNote
DROP TABLE tblReportTaskFile
DROP TABLE tblFeedbackTask
DROP TABLE tblNotification

INSERT INTO tblRole (sRoleName) 
VALUES 
    (N'Quản trị viên'),
    (N'Người dùng')

SELECT *FROM tblUser

ALTER TABLE tblReportTaskFile
ADD sSenderName NVARCHAR(255),
    sReportDescription NVARCHAR(1000);

ALTER TABLE tblGroupTaskFile
ADD sDescription NVARCHAR(1000);

UPDATE tblUser
SET iRoleID = 1
WHERE iUserID = 4