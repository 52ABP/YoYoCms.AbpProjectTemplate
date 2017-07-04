<!--发送激活邮件-->
<style rel="styleesheet" lang="scss">
    .sendactiveemail-container {
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
    <article class="sendactiveemail-container">
        <article class="login-page">
            <div class="login-box">
                <div class="logo">
                    <a href="javascript:void(0);">Abp<b>ProjectTemplate</b></a>
                    <small>Admin BootStrap Based - Material Design</small>
                </div>
                <div class="card">
                    <div class="body" v-loading="loading">
                        <div class="msg">发送激活邮件</div>
                        <div class="row">
                            <i class="col-xs-12" style="color: #333; font-size: 14px">
                                系统在几十秒内向您发送一封邮件,用于激活您的用户账号，请接收并点击邮件内容中的激活链接。如果在2分钟内还没收到这封邮件，请重试.
                            </i>
                        </div>
                        <div class="row">
                            <i class="col-xs-12">
                                <input type="email" @keyup.enter="submit" v-model="fetchParam.emailAddress"
                                       class="form-control" placeholder="邮箱地址" ref="txtEmail">
                            </i>
                        </div>
                        <div class="row">
                            <div class="col-xs-4 col-xs-offset-8">
                                <button @click="submit" class="btn btn-block bg-pink waves-effect" type="submit">提交
                                </button>
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
    //    import authUtils from '../../common/utils/authUtils'
    export default {
        data() {
            return {
                loading: false,
                fetchParam: {
                    emailAddress: '',
                },
            }
        },
        mounted() {
            this.$refs.txtEmail.focus()
        },
        methods: {
            async submit () {
                this.loading = true

                try {
                    await userService.sendEmailActivationLink(this.fetchParam)
                    abp.message.confirm('已向您的邮箱发送了一封激活邮件，请查收', '邮件已发送', (ret) => {
                        if (ret) this.$router.replace({name: 'login'})
                    })
                } finally {
                    this.loading = false
                }
            },
        },
        components: {}
    }
</script>