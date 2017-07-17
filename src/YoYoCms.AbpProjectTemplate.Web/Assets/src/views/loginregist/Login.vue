<style rel="styleesheet" lang="scss">
    .login-container {
        background: #00BCD4;
        position: absolute;
        width: 100%;
        height: 100%;
        .el-form-item__content {
            line-height: 0
        }
    }
</style>
<template>
    <article class="login-container">
        <article class="login-page">
            <div class="login-box">
                <div class="logo">
                    <a href="javascript:void(0);">Abp<b>ProjectTemplate</b></a>
                    <small>Admin BootStrap Based - Material Design</small>
                </div>
                <div class="card">
                    <div class="body" v-loading="loading">
                        <div class="msg">Sign in to start your session</div>
                        <el-form ref="form" :model="fetchParam" :rules="rules" label-width="0"
                                 class="demo-ruleForm">
                            <el-form-item label="" prop="usernameOrEmailAddress">
                                <div class="input-group">
                                <span class="input-group-addon">
                                    <i class="material-icons">person</i>
                                </span>
                                    <div class="form-line">
                                        <input @focus type="text" class="form-control" name="username"
                                               :placeholder="L('UserNameOrEmail')"
                                               v-model="fetchParam.usernameOrEmailAddress" ref="txtUsername"
                                               required
                                               autofocus>
                                    </div>
                                </div>
                            </el-form-item>

                            <el-form-item prop="usernameOrEmailAddress">
                                <div class="input-group">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                                    <div class="form-line">
                                        <input type="password" @keyup.enter="login" v-model="fetchParam.password"
                                               class="form-control" :placeholder="L('Password')"
                                               required>
                                    </div>
                                </div>
                            </el-form-item>
                        </el-form>
                        <div class="row">
                            <div class="col-xs-8 p-t-5">
                                <input type="checkbox" id="rememberme" v-model="fetchParam.rememberMe"
                                       class="filled-in chk-col-pink">
                                <label for="rememberme">{{L('RememberMe')}}</label>
                            </div>
                            <div class="col-xs-4">
                                <button @click="login" class="btn btn-block bg-pink waves-effect" type="submit">
                                    {{L('LogIn')}}
                                </button>
                            </div>
                        </div>
                        <div class="row m-t-15 m-b--20">
                            <div class="col-xs-4">
                                <a @click="$router.push({name:'register'})"
                                   style="cursor:pointer;">{{L('CreateAnAccount')}}</a>
                            </div>
                            <div class="col-xs-4" style="text-align: center">
                                <a @click="$router.push({name:'SendActiveEmail'})"
                                   style="cursor:pointer;">{{L('EmailActivation')}}</a>
                            </div>
                            <div class="col-xs-4 align-right">
                                <a @click="$router.push({name: 'forgetPwd'})"
                                   style="cursor: pointer">{{L('ForgotPassword')}}</a>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-4">
                                <Languages></Languages>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </article>
    </article>
</template>

<script>
    //    import config from '../../common/config'
    import userService from '../../services/userService'
    import sessionService from '../../services/sessionService'
    import Languages from './components/Languages.vue'
    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            return {
                loading: false,
                fetchParam: {
                    usernameOrEmailAddress: '',
                    password: '',
                    tenancyName: 'default',
                    rememberMe: false
                },
                rules: {
                    usernameOrEmailAddress: [
                        {type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'change'},
                    ],
                    password: [
                        {type: 'string', required: true, message: lang.L('RequiredFiled'), trigger: 'blur'}
                    ],
                }
            }
        },
        mounted() {
            window.initAdminJs && window.initAdminJs()
            this.$refs.txtUsername.focus()
        },
        methods: {
            login () {
                this.$refs.form.validate(async (valid) => {
                    if (!valid) return
                    this.loading = true
                    try {
                        let ret = await userService.login(this.fetchParam)

                        // 如果需要重新设置密码
                        if (ret.result && ret.result.resetPassword) {
                            abp.notify.success(lang.L('PasswordResetEmail_SubTitle'), lang.L('LoginSuccessful'))
                            delete ret.result.resetPassword
                            this.$router.push({name: 'resetpassword', query: ret.result})
                            return
                        }
                        setTimeout(async () => {
                            // 获取用户信息
                            let user = (await sessionService.getCurrentLoginInformations()).user
                            this.$store.dispatch('setAuthUser', {user})
                            authUtils.setUserInfo(user)

                            this.$router.push({name: 'Dashboard.Tenant'})
                            abp.notify.success(lang.L('LoginSuccessful'), lang.L('Success'))
//                            this.loading = false
                        }, 5e2)
                    } catch (e) {
                        abp.notify.error(lang.L('UserNameOrPasswordError'), lang.L('LoginFailed'))
                        this.loading = false
                    }
                })
            },
        },
        components: {Languages}
    }
</script>