<style rel="styleesheet" lang="scss">
.index-container {
    position: absolute;
    min-height: 100%;
    width: 100%;
    background-color: #e9e9e9;
    overflow-x: hidden;
    .navbar {
        position: fixed;
        border-radius: 0 !important;
        max-height: 73px;
    }

    #leftsidebar {
        .dropdown-menu>li>a {
            height: 46px;
            line-height: 46px !important;
            .material-icons {
                vertical-align: middle;
                float: none;
            }
        }
    }
}
</style>

<template>
    <article class="theme-red index-container">
        <!-- Overlay For Sidebars -->
        <div class="overlay"></div>
        <!-- #END# Overlay For Sidebars -->
        <!-- #END# Search Bar -->
        <!-- Top Bar -->
        <nav class="navbar">
            <div class="container-fluid">
                <div class="navbar-header">
                    <a href="javascript:void(0);" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#navbar-collapse" aria-expanded="false"></a>
                    <a href="javascript:void(0);" class="bars"></a>
                    <a class="navbar-brand" href="../../index.html">YoYoCms - MATERIAL 设计的后台管理系统</a>
                </div>
                <div class="collapse navbar-collapse" id="navbar-collapse">
                    <ul class="nav navbar-nav navbar-right">
                        <!-- #END# Call Search -->
                        <!-- Notifications -->
                        <li class="dropdown">
                            <a href="javascript:void(0);" class="dropdown-toggle" data-toggle="dropdown" role="button">
                                <i class="material-icons">notifications</i>
                                <span class="label-count">7</span>
                            </a>
                            <ul class="dropdown-menu">
                                <li class="header">通知</li>
                                <li class="body">
                                    <ul class="menu">
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-light-green">
                                                    <i class="material-icons">person_add</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>12 new members joined</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> 14 mins ago
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-cyan">
                                                    <i class="material-icons">add_shopping_cart</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>4 sales made</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> 22 mins ago
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-red">
                                                    <i class="material-icons">delete_forever</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>
                                                        <b>Nancy Doe</b> deleted account</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> 3 hours ago
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-orange">
                                                    <i class="material-icons">mode_edit</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>
                                                        <b>Nancy</b> changed name</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> 2 hours ago
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-blue-grey">
                                                    <i class="material-icons">comment</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>
                                                        <b>John</b> commented your post</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> 4 hours ago
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-light-green">
                                                    <i class="material-icons">cached</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>
                                                        <b>John</b> updated status</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> 3 hours ago
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <div class="icon-circle bg-purple">
                                                    <i class="material-icons">settings</i>
                                                </div>
                                                <div class="menu-info">
                                                    <h4>Settings updated</h4>
                                                    <p>
                                                        <i class="material-icons">access_time</i> Yesterday
                                                    </p>
                                                </div>
                                            </a>
                                        </li>
                                    </ul>
                                </li>
                                <li class="footer">
                                    <a href="javascript:void(0);">View All Notifications</a>
                                </li>
                            </ul>
                        </li>
                        <!-- #END# Notifications -->
                        <!-- Tasks -->
                        <li class="dropdown">
                            <a href="javascript:void(0);" class="dropdown-toggle" data-toggle="dropdown" role="button">
                                <i class="material-icons">flag</i>
                                <span class="label-count">9</span>
                            </a>
                            <ul class="dropdown-menu">
                                <li class="header">任务</li>
                                <li class="body">
                                    <ul class="menu tasks">
                                        <li>
                                            <a href="javascript:void(0);">
                                                <h4>
                                                    Footer display issue
                                                    <small>32%</small>
                                                </h4>
                                                <div class="progress">
                                                    <div class="progress-bar bg-pink" role="progressbar" aria-valuenow="85" aria-valuemin="0" aria-valuemax="100" style="width: 32%">
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <h4>
                                                    Make new buttons
                                                    <small>45%</small>
                                                </h4>
                                                <div class="progress">
                                                    <div class="progress-bar bg-cyan" role="progressbar" aria-valuenow="85" aria-valuemin="0" aria-valuemax="100" style="width: 45%">
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <h4>
                                                    Create new dashboard
                                                    <small>54%</small>
                                                </h4>
                                                <div class="progress">
                                                    <div class="progress-bar bg-teal" role="progressbar" aria-valuenow="85" aria-valuemin="0" aria-valuemax="100" style="width: 54%">
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <h4>
                                                    Solve transition issue
                                                    <small>65%</small>
                                                </h4>
                                                <div class="progress">
                                                    <div class="progress-bar bg-orange" role="progressbar" aria-valuenow="85" aria-valuemin="0" aria-valuemax="100" style="width: 65%">
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                        <li>
                                            <a href="javascript:void(0);">
                                                <h4>
                                                    Answer GitHub questions
                                                    <small>92%</small>
                                                </h4>
                                                <div class="progress">
                                                    <div class="progress-bar bg-purple" role="progressbar" aria-valuenow="85" aria-valuemin="0" aria-valuemax="100" style="width: 92%">
                                                    </div>
                                                </div>
                                            </a>
                                        </li>
                                    </ul>
                                </li>
                                <li class="footer">
                                    <a href="javascript:void(0);">View All Tasks</a>
                                </li>
                            </ul>
                        </li>
                        <!-- #END# Tasks -->
                        <li class="pull-right">
                            <a href="javascript:void(0);" class="js-right-sidebar" data-close="true">
                                <i class="material-icons">more_vert</i>
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </nav>
        <!-- #Top Bar -->
        <section>
            <!-- Left Sidebar -->
            <aside id="leftsidebar" class="sidebar">
                <!-- User Info -->
                <div class="user-info">
                    <div class="image">
                        <img :src="user.portrait" width="48" height="48" alt="User" />
                    </div>
                    <div class="info-container">
                        <div class="name" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                            {{user.name}}
                        </div>
                        <div class="email">{{user.emailAddress}}</div>
                        <div class="btn-group user-helper-dropdown">
                            <i class="material-icons" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">keyboard_arrow_down</i>
                            <ul class="dropdown-menu pull-right">
                                <li>
                                    <a @click="dialogMe.isShow= true">
                                        <i class="material-icons">person</i>个人中心</a>
                                </li>
                                <li>
                                    <a @click="dialogPwd.isShow= true">
                                        <i class="material-icons">vpn_key</i>修改密码</a>
                                </li>
                                <li>
                                    <a @click="dialogPortrait.isShow= true">
                                        <i class="material-icons">image</i>修改头像</a>
                                </li>
                                <li role="seperator" class="divider"></li>
                                <li @click="logout">
                                    <a href="javascript:void(0);">
                                        <i class="material-icons">input</i>注销</a>
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
                <!-- #User Info -->
                <!-- Menu -->
                <div class="menu">
                    <ul class="list">
                        <li class="header">狂拽炫酷吊炸天的超强功能后台管理系统</li>
                        <MenuTree v-for="(item,index) in menus.items" :menu="item" :key="index"></MenuTree>
                    </ul>
                </div>
                <!-- #Menu -->
                <!-- Footer -->
                <div class="legal">
                    <div class="copyright">
                        &copy; 2017
                        <a href="javascript:void(0);">AdminBSB - Material Design</a>.
                    </div>
                    <div class="version">
                        <b>Version: </b> 1.0.4
                    </div>
                </div>
                <!-- #Footer -->
            </aside>
            <!-- #END# Left Sidebar -->
        </section>
        <!--内容部分-->
        <section class="content">
            <Nav></Nav>
            <router-view></router-view>
        </section>
    
        <!--个人信息弹窗-->
        <DialogProfile :visible.sync="dialogMe.isShow"></DialogProfile>
        <!--修改密码弹窗-->
        <DialogEditPwd :visible.sync="dialogPwd.isShow"></DialogEditPwd>
        <!--修改头像弹出框-->
        <DialogPortrait :visible.sync="dialogPortrait.isShow"></DialogPortrait>
    </article>
</template>

<script>
import '../vendor/bsb/plugin/jquery-slimscroll/jquery.slimscroll'
//    import authUtils from '../common/utils/authUtils'
import loadFile from '../common/utils/loadFile'
import abpScriptService from '../services/abpScriptService'
import sessionService from '../services/sessionService'
import userService from '../services/userService'

import MenuTree from '../components/menu/MenuTree.vue' // 左边菜单
import Nav from './components/Nav.vue' // 内容上部的导航栏
import DialogProfile from './components/DialogProfile.vue' // 修改个人信息 弹出框
import DialogEditPwd from './components/DialogEditPassword.vue' // 修改密码 弹出框
import DialogPortrait from './components/DialogPortrait.vue' // 修改头像 弹出框
export default {
    data() {
        return {
            menus: [],
            user: this.$store.state.auth.user,
            dialogMe: { // 修改个人信息
                isShow: false
            },
            dialogPwd: { isShow: false },
            dialogPortrait: { isShow: false }
        }
    },
    watch: {
        '$store.state.auth.user'(val) {
            this.user = val
        }
    },
    async created() {
        // 如果用户信息没获取到
        if (!this.user.id) {
            let ret = await sessionService.getCurrentLoginInformations()
            let user = ret.user
            this.$store.dispatch('setAuthUser', { user })
            //                authUtils.setUserInfo(user)
            this.user = user
        }
    },
    activated() {
    },
    async mounted() {
        // 获取菜单信息
        await abpScriptService.getScripts()
        this.menus = abp.nav.menus.MainMenu
        // 刷新当前激活菜单的信息
        this.$store.dispatch('setIndexMenuActive', { menu: this.$store.state.index.navMenueActive })
        this.$nextTick(async () => {
            //                await Promise.all(loadFile.loadJs(require('../vendor/bsb/js/demo')), loadFile.loadJs(require('../vendor/bsb/js/admin')))
            await loadFile.loadJs(require('../vendor/bsb/js/demo'))
            await loadFile.loadJs(require('../vendor/bsb/js/admin'))
            window.initDemoJs()
            window.initAdminJs()
            this.loading = false
        })
    },
    methods: {
        // 登出
        logout() {
            userService.logout()
            abp.nav = null
            //                authUtils.setToken('')
            this.$router.push({ name: 'login' })
            abp.notify.success('已成功退出登录', '提示')
        }
    },
    components: { MenuTree, Nav, DialogProfile, DialogEditPwd, DialogPortrait }
}
</script>
