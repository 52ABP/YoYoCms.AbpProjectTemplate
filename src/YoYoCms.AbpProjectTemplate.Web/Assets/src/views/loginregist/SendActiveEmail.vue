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
                        <div class="msg">{{L('EmailActivation')}}</div>
                        <div class="row">
                            <i class="col-xs-12" style="color: #333; font-size: 14px">
                                {{L('SendEmailActivationLink_Information')}}
                            </i>
                        </div>
                        <div class="row">
                            <i class="col-xs-12">
                                <input type="email" @keyup.enter="submit" v-model="fetchParam.emailAddress"
                                       class="form-control" :placeholder="L('EmailAddress')" ref="txtEmail">
                            </i>
                        </div>
                        <div class="row">
                            <div class="col-xs-4 col-xs-offset-8">
                                <button @click="submit" class="btn btn-block bg-pink waves-effect" type="submit">
                                    {{L('Submit')}}
                                </button>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6">
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
    import Languages from './components/Languages.vue'
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
                    abp.message.confirm(lang.L('ActivationMailSentMessage'), lang.L('MailSent'), (ret) => {
                        if (ret) this.$router.replace({name: 'login'})
                    })
                } finally {
                    this.loading = false
                }
            },
        },
        components: {Languages}
    }
</script>