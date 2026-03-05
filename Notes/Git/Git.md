# Git



> [!TIP]
>
> **一个正常的项目中，所有长期存在的、共享的分支都应该有且共享同一份 `.gitignore` 文件**
>
> （同目录下放了一份通用`.gitignore` 文件，以作备份）







## Git submodule

`Git Submodule` 是一种在 `Git` 仓库内部嵌套其他 `Git` 仓库的方式。当项目依赖于其他独立维护的第三方库或组件时，可以将这些库或组件作为一个子模块添加到项目中。这样一来，主项目的 `Git` 仓库中仅存储子模块的引用信息（包括仓库地址和对应的提交哈希），而子模块的实际源代码则存储在其各自的 `Git` 仓库中。

**Git Submodule 的基本用法**

**添加子模块**：

通过 `git submodule add` 命令添加子模块，格式如下：

```bash
git submodule add <子模块仓库URL> <本地路径>
```

例如，若要将 GitHub 上的某个仓库作为子模块添加到当前项目，执行：

```bash
git submodule add https://github.com/user/repo.git path/to/submodule
```

**初始化和更新子模块**：

首次添加子模块后，需要运行以下命令来下载子模块并检出指定的提交：

```bash
git submodule init
git submodule update
```

或者，可以简写为：

```bash
git submodule update --init
```

若要同时更新所有子模块的子模块（递归更新），需加上 `--recursive` 参数：

```bash
git submodule update --init --recursive
```

**日常管理子模块**：

- 检查子模块的状态：`git submodule status`
- 更新子模块到最新提交：`git submodule update`
- 进入子模块目录进行操作：`cd path/to/submodule`