<style rel="styleesheet" lang="scss">
</style>
<template>
    <article class="login-page">
        <div class="login-box">
            <div class="logo">
                <a href="javascript:void(0);">Admin<b>BSB</b></a>
                <small>Admin BootStrap Based - Material Design</small>
            </div>
            <div class="card">
                <div class="body">
                    <form id="sign_in" method="POST">
                        <div class="msg">Sign in to start your session</div>
                        <div class="input-group">
                        <span class="input-group-addon">
                            <i class="material-icons">person</i>
                        </span>
                            <div class="form-line">
                                <input type="text" class="form-control" name="username" placeholder="Username" required autofocus>
                            </div>
                        </div>
                        <div class="input-group">
                        <span class="input-group-addon">
                            <i class="material-icons">lock</i>
                        </span>
                            <div class="form-line">
                                <input type="password" class="form-control" name="password" placeholder="Password" required>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-8 p-t-5">
                                <input type="checkbox" name="rememberme" id="rememberme" class="filled-in chk-col-pink">
                                <label for="rememberme">Remember Me</label>
                            </div>
                            <div class="col-xs-4">
                                <button class="btn btn-block bg-pink waves-effect" type="submit">SIGN IN</button>
                            </div>
                        </div>
                        <div class="row m-t-15 m-b--20">
                            <div class="col-xs-6">
                                <a href="sign-up.html">Register Now!</a>
                            </div>
                            <div class="col-xs-6 align-right">
                                <a href="forgot-password.html">Forgot Password?</a>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </article>
</template>

<script>
    //    import config from '../../common/config'
    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            return {
                fetchParam: {
                    usernameOrEmailAddress: 'admin',
                    password: 'bb123456',
                    tenancyName: 'default'
                }
            }
        },
        created() {
        },
        activated() {
        },
        methods: {
            async login () {
                let ret = await abp.ajax({
                    url: `/api/account?returnUrl=application`,
                    method: 'post',
                    data: JSON.stringify(this.fetchParam)
                })

                if (!ret) return
                authUtils.setToken(ret.result)
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