<!--消息设置-->
<style rel="styleesheet" lang="scss">
    .dialog__notifi-setting {
        .el-dialog {
            padding-bottom: 15px;
        }

        .dialog__notifi-setting--content{
            h4{
                font-size: 18px;
            }
            h4,h5{
                font-weight: normal;
            }
        }
    }
</style>

<template>
    <el-dialog class="dialog__notifi-setting"
            :title="L('NotificationSettings')"
            :visible.sync="dialogVisible"
               @open="handleOpen"
            size="tiny">
        <article v-loading="loading" class="dialog__notifi-setting--content">
            <section>
                <h4>
                    [{{L('ReceiveNotifications')}}]
                </h4>
                <em>
                    <el-switch
                            v-model="receiveNotifications"
                            :on-text="L('On')"
                            :off-text="L('Off')">
                    </el-switch>
                </em>
            </section>
            <section style="margin-top: 25px">
                <h4>
                    [{{L('NotificationtType')}}]
                </h4>
            </section>

            <section v-for="item in notifications">
                <h5>
                    <el-checkbox v-model="item.isSubscribed">{{item.displayName}}</el-checkbox>
                </h5>
            </section>
        </article>
        <span slot="footer" class="dialog-footer">
            <el-button class="waves-effect" @click="dialogVisible = false">{{L('Cancel')}}</el-button>
            <el-button class="waves-effect" type="primary" @click="save">{{L('Save')}}</el-button>
        </span>
    </el-dialog>
</template>

<script>
    import notifiService from '../../services/common/notificationService'
    export default {
        props: {
            visible: Boolean,
        },
        data() {
            return {
                loading: false,
                receiveNotifications: void 0,
                notifications: [],
                dialogVisible: false
            }
        },
        watch: {
            'visible' (val) {
                if (val != this.dialogVisible) this.dialogVisible = val
            },
            'dialogVisible' (val) {
                this.$emit('update:visible', val)
            }
        },
        async activated() {
        },
        async mounted () {
        },
        methods: {
            async handleOpen () {
                this.loading = true
                let ret = await notifiService.getNotificationSettings()
                this.receiveNotifications = ret.receiveNotifications
                this.notifications = ret.notifications
                this.loading = false

                console.log(ret)
            },
            async save() {
                this.loading = true
                await notifiService.updateNotificationSettings({
                    receiveNotifications: this.receiveNotifications,
                    notifications: this.notifications
                })
                abp.notify.success(lang.L('SavedSuccessfully'), lang.L('Success'))
                this.loading = false
                this.dialogVisible = false
            }
        },
        components: {}
    }
</script>

