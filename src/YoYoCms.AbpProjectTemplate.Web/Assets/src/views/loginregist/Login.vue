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
                                        <input type="text" class="form-control" name="username" placeholder="用户名或邮箱地址"
                                               v-model="fetchParam.usernameOrEmailAddress"
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
                                               class="form-control" placeholder="密码"
                                               required>
                                    </div>
                                </div>
                            </el-form-item>
                        </el-form>
                        <div class="row">
                            <div class="col-xs-8 p-t-5">
                                <input type="checkbox" name="rememberme" id="rememberme"
                                       class="filled-in chk-col-pink">
                                <label for="rememberme">记住我</label>
                            </div>
                            <div class="col-xs-4">
                                <button @click="login" class="btn btn-block bg-pink waves-effect" type="submit">登录
                                </button>
                            </div>
                        </div>
                        <div class="row m-t-15 m-b--20">
                            <div class="col-xs-6">
                                <a href="sign-up.html">立即注册!</a>
                            </div>
                            <div class="col-xs-6 align-right">
                                <a href="forgot-password.html">忘记密码?</a>
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
    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            return {
                loading: false,
                fetchParam: {
                    usernameOrEmailAddress: '',
                    password: '',
                    tenancyName: 'default'
                },
                rules: {
                    usernameOrEmailAddress: [
                        {type: 'string', required: true, message: '请输入邮箱或用户名', trigger: 'change'},
                    ],
                    password: [
                        {type: 'string', required: true, message: '请输入密码', trigger: 'blur'}
                    ],
                }
            }
        },
        created() {
        },
        activated() {
        },
        methods: {
            async login () {
                this.$refs.form.validate(async (valid) => {
                    if (!valid) return

                    this.loading = true
                    let ret = await abp.ajax({
                        url: `/api/account?returnUrl=application`,
                        method: 'post',
                        data: JSON.stringify(this.fetchParam)
                    }).catch(() => {
                        this.loading = false
                    })

                    this.loading = false
                    if (!ret) return
                    authUtils.setToken(ret)
                    abp.notify.success('登录成功!', '恭喜')

                    this.$router.push({name: 'index'})
                })
            },
            async testGetData () {
                // 再请求一个借口
                let retRole = await abp.services.app.role.getRoles({permission: ''}).catch((...e) => {
                    console.log(e)
                })
                console.log(retRole)
            }
        },
        components: {}
    }
</script>