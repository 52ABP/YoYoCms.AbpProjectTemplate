<!--右上角的通知-->
<style rel="styleesheet" lang="scss">
    .index-component__right-notification {
        .header {
            cursor: pointer;
            &:hover {
                background: #f8f8f8;
            }
        }
        .notification-item {
            &.notification-item_reded {
                h4 {
                    color: #999 !important;
                }
            }
            h4 {
                font-size: 14px !important;
                margin-bottom: 3px !important;
            }

            .icon-circle {
                i {
                    font-size: 24px !important;
                }
            }
        }
    }
</style>

<template>
    <li class="index-component__right-notification">
        <a href="javascript:void(0);" class="dropdown-toggle" data-toggle="dropdown" role="button">
            <i class="material-icons">notifications</i>
            <span class="label-count">{{notRead}}</span>
        </a>
        <ul class="dropdown-menu">
            <li class="header" @click.stop="setAllReaded">
                {{L('SetAllAsRead')}}
            </li>
            <li class="body">
                <ul class="menu">
                    <li v-for="item in data" :key="item.id" class="notification-item"
                        :class="{'notification-item_reded': item.state == 1}">
                        <a href="javascript:void(0);">
                            <div class="icon-circle" :class="{'bg-light-green': item.notification.severity == 1, 'bg-orange':item.notification.severity == 2,
                            'bg-purple':item.notification.severity == 4, 'bg-blue-grey':item.notification.severity == 0, 'bg-red':item.notification.severity == 3}">
                                <!--info-->
                                <i class="material-icons" v-if="item.notification.severity == 0">info</i>
                                <!--success-->
                                <i class="material-icons" v-if="item.notification.severity == 1">done</i>
                                <!--warn-->
                                <i class="material-icons" v-if="item.notification.severity == 2">warning</i>
                                <!--error-->
                                <i class="material-icons" v-if="item.notification.severity == 3">cancel</i>
                                <!--fatal-->
                                <i class="material-icons" v-if="item.notification.severity == 4">do_not_disturb_on</i>
                            </div>
                            <div class="menu-info">
                                <h4>{{item.notification.data.message}}</h4>
                                <p>
                                    <i class="material-icons">access_time</i>
                                    {{item.notification.creationTime | date2calendar}}

                                    <el-button @click.stop="setReaded(item)" v-if="item.state == 0" type="text"
                                               style="font-size: 12px" size="tiny">
                                        {{L('SetAsRead')}}
                                    </el-button>
                                </p>
                            </div>
                        </a>
                    </li>
                </ul>
            </li>
            <li class="footer" @click="$router.push({name:'common-notifications'})">
                <a href="javascript:void(0);">{{L('SeeAllNotifications')}}</a>
            </li>
        </ul>
    </li>
</template>

<script>
    import notificationService from '../../services/common/notificationService'
    export default {
        data() {
            return {
                data: this.$store.state.index.notifications,
                notRead: this.$store.state.index.unReadNotification, // 未读数量
            }
        },
        watch: {
            '$store.state.index.notifications'(val) {
                this.data = val
                this.$nextTick(() => {
                    $.AdminBSB && $.AdminBSB.dropdownMenu && $.AdminBSB.dropdownMenu.activate()
                })
            },
            '$store.state.index.unReadNotification'(val) {
                this.notRead = val
            }
        },
        async created() {
            let ret = await notificationService.getUserNotifications({maxResultCount: 10})
            this.$store.dispatch('setUnreadNotification', {count: ret.unreadCount})
            this.$store.dispatch('setNotifications', {data: ret.items})
            abp.event.on('abp.notifications.received', (userNotification) => {
                this.$store.dispatch('pushNofications', {data: userNotification})
            })

            abp.event.on('app.notifications.read', (userNotificationId) => {
                console.log('app.notifications.read', 'notification.vue')
                this.$store.dispatch('setNotificationReaded', {id: userNotificationId})
            })
        },
        activated() {
        },
        methods: {
            // 设为已读
            async setReaded(item) {
                await notificationService.setNotificationAsRead({id: item.id})
                this.$store.dispatch('setNotificationReaded', {data: item})
            },
            // 设置所有为已读
            async setAllReaded() {
                await notificationService.setAllNotificationsAsRead()
                this.data.forEach((item) => {
                    this.$store.dispatch('setNotificationReaded', {data: item})
                })
            }
        },
        components: {}
    }
</script>
