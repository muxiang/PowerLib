# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

PowerLib 是一个基于 WPF 和 WinForms 的 UI 组件库，提供主题切换、动画效果和多种扩展控件。采用 MIT 许可证，通过 NuGet 发布。

## 技术栈

- 语言：C# 7.3（使用最新语言特性）
- 目标框架：.NET Framework 4.5（net45）
- SDK：.NET 9.0
- UI 框架：WPF 和 WinForms
- 构建工具：MSBuild / dotnet CLI
- 版本：1.0.10

## 项目结构

解决方案包含 5 个项目：

| 项目 | 类型 | 描述 |
|------|------|------|
| **PowerLib** | 类库（基础） | 核心程序集，包含 P/Invoke 定义、原语类型和通用工具 |
| **PowerLib.WPF** | 类库（WPF） | WPF 控件库，依赖 PowerLib |
| **PowerLib.WinForms** | 类库（WinForms） | WinForms 控件库，依赖 PowerLib |
| **PowerLib.WPF.Samples** | 应用程序 | WPF 示例程序 |
| **PowerLib.WinForms.Samples** | 应用程序 | WinForms 示例程序 |

关键目录：
- `PowerLib/NativeCodes/` - P/Invoke 声明（NativeMethods, NativeConstants, NativeStructures）
- `PowerLib/Primitives/` - 平台无关的原语类型（Color, Point, Rectangle, Size）
- `PowerLib.WPF/Controls/` - WPF 自定义控件
- `PowerLib.WPF/Themes/` - 主题和样式文件
- `PowerLib.WPF/Tools/` - 转换器等辅助工具
- `SlnRelease/` - 构建产物输出目录

## 构建命令

```bash
# 还原依赖
dotnet restore PowerLib.sln

# 构建整个解决方案（Debug）
dotnet build PowerLib.sln

# 构建整个解决方案（Release）
dotnet build PowerLib.sln --configuration Release

# 构建单个项目
dotnet build PowerLib/PowerLib.csproj --configuration Release
dotnet build PowerLib.WPF/PowerLib.WPF.csproj --configuration Release

# 创建 NuGet 包
dotnet pack PowerLib/PowerLib.csproj --configuration Release
dotnet pack PowerLib.WPF/PowerLib.WPF.csproj --configuration Release
dotnet pack PowerLib.WinForms/PowerLib.WinForms.csproj --configuration Release

# 运行示例程序
dotnet run --project PowerLib.WPF.Samples/PowerLib.WPF.Samples.csproj
dotnet run --project PowerLib.WinForms.Samples/PowerLib.WinForms.Samples.csproj
```

构建产物位置：
- DLL/PDB：`{Project}/bin/Release/net45/`
- NuGet 包：`{Project}/bin/Release/*.nupkg`
- 合并发布：`SlnRelease/`

## 关键架构信息

### WPF 主题系统

WPF 项目使用 **XAML Combine 工具**在构建前自动合并主题文件：

1. `Themes/ThemeFileList.txt` 定义要合并的 XAML 文件列表
2. 构建时自动运行 `Themes/XamlCombine.exe` 将所有样式合并到 `Generic.xaml`
3. 支持 3 种颜色主题：Default（默认）、Dark（暗黑）、Pink（粉色）
4. 主题颜色定义在 `Themes/Basic/Colors/` 目录下

重要：修改样式后需要重新构建才能看到变化。

### 控件设计模式

WPF 控件遵循标准 WPF 模式：
- 继承自 `ContentControl`、`Control` 或相应基类
- 使用 `TemplatePart` 特性声明控件部件
- 使用依赖属性支持数据绑定和样式
- 实现 `IDisposable` 管理资源（如动画定时器）

主要控件：
- **Window** - 自定义窗口，支持阴影、圆角、标题栏自定义
- **LoadingCircle** - Win8 风格加载动画
- **LoadingLine** - 线性加载动画
- **LoadingLayer** - 加载遮罩层
- **FileTransferProgressBar** - Win10 风格文件传输进度条
- **RipplePictureBox** - 水波纹效果图片框
- **HotKeyRecorder** - 热键录制控件
- **IPV4InputBox** - IPv4 地址输入框
- **EnhancedProgressBar** - 增强型进度条

### P/Invoke 层

PowerLib 核心库提供 Windows API 封装：
- `NativeMethods` - Windows API 方法声明
- `NativeConstants` - Windows 消息和常量定义
- `NativeStructures` - Windows 结构体定义

### 平台无关原语

`PowerLib/Primitives/` 包含可跨 UI 框架共享的类型：
- `Color` - RGBA 颜色结构
- `Point` - 二维点
- `Rectangle` - 矩形
- `Size` - 尺寸

## NuGet 发包流程

每个可打包项目（PowerLib、PowerLib.WPF、PowerLib.WinForms）的 csproj 都配置了：
1. 从 `RELEASE_NOTE.md` 读取发布说明
2. 包含 `icon.png` 和 `README.md` 到包根目录
3. 生成 XML 文档文件

发布新版本时：
1. 更新对应项目的 `RELEASE_NOTE.md`
2. 更新 csproj 中的 `<Version>` 标签
3. 运行 `dotnet pack --configuration Release`
4. 推送到 NuGet：`dotnet nuget push {Package}.nupkg --api-key {API_KEY} --source https://api.nuget.org/v3/index.json`

## 测试

目前没有独立的测试项目。测试通过示例程序进行：
- `PowerLib.WPF.Samples` - WPF 控件功能演示
- `PowerLib.WinForms.Samples` - WinForms 控件功能演示

## 注意事项

1. 所有项目目标框架为 **net45**（.NET Framework 4.5），不是 .NET Core/.NET 5+
2. 使用 `AllowUnsafeBlocks`，项目中有 unsafe 代码（主要在 P/Invoke 和 GDI+ 操作中）
3. WPF 项目的构建依赖 `XamlCombine.exe`，确保该工具存在于 `Themes/` 目录
4. 文档注释使用 XML 格式（`GenerateDocumentationFile` 已启用）
5. 使用 file-scoped namespace 语法（C# 10 特性，但目标框架仍是 net45）
