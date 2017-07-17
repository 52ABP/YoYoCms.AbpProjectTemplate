<!--树形菜单-->
<!--<ul class="list">-->
<!--<li class="active">-->
<!--<a href="javascript:void(0);" class="menu-toggle">-->
<!--<i class="material-icons">widgets</i>-->
<!--<span>Widgets</span>-->
<!--</a>-->
<!--<ul class="ml-menu">-->
<!--<li class="active">-->
<!--<a href="javascript:void(0);" class="menu-toggle">-->
<!--<span>Cards</span>-->
<!--</a>-->
<!--<ul class="ml-menu">-->
<!--<li class="active">-->
<!--<a href="../../pages/widgets/cards/basic.html">Basic</a>-->
<!--</li>-->
<!--<li>-->
<!--<a href="../../pages/widgets/cards/colored.html">Colored</a>-->
<!--</li>-->
<!--<li>-->
<!--<a href="../../pages/widgets/cards/no-header.html">No Header</a>-->
<!--</li>-->
<!--</ul>-->
<!--</li>-->
<!--<li>-->
<!--<a href="javascript:void(0);" class="menu-toggle">-->
<!--<span>Infobox</span>-->
<!--</a>-->
<!--<ul class="ml-menu">-->
<!--<li>-->
<!--<a href="../../pages/widgets/infobox/infobox-1.html">Infobox-1</a>-->
<!--</li>-->
<!--<li>-->
<!--<a href="../../pages/widgets/infobox/infobox-2.html">Infobox-2</a>-->
<!--</li>-->
<!--<li>-->
<!--<a href="../../pages/widgets/infobox/infobox-3.html">Infobox-3</a>-->
<!--</li>-->
<!--<li>-->
<!--<a href="../../pages/widgets/infobox/infobox-4.html">Infobox-4</a>-->
<!--</li>-->
<!--<li>-->
<!--<a href="../../pages/widgets/infobox/infobox-5.html">Infobox-5</a>-->
<!--</li>-->
<!--</ul>-->
<!--</li>-->
<!--</ul>-->
<!--</li>-->
<!--</ul>-->
<style rel="styleesheet" lang="scss">
    .menutree-item {
        &.menutree-item-leaf {
            .menutree-link:before {
                content: none !important;
            }
        }

        &.active {
            > a {
                margin-left: 0 !important;
            }
        }

        .menutree-link {
            cursor: pointer;

            > span {
                padding-top: 4px;
            }
        }
    }

</style>

<template>
    <li v-if="menu" :class="{active: isActive, 'menutree-item-leaf': isLeaf}" class="menutree-item">
        <a class="menutree-link" @click="jumpUrl" :class="{'menu-toggle': !isLeaf}">
            <i class="material-icons" v-if="deep == 0">{{icons[deep]}}</i>
            <span>{{menu.displayName}}</span>
        </a>
        <ul class="ml-menu" v-if="menu.items && menu.items.length > 0">
            <MenuTree v-for="(item,index) in menu.items" :menu="item" :deep="deep+1" :key="index"></MenuTree>
        </ul>
    </li>
</template>

<script>
    import MenuTree from './MenuTree.vue'
    export default {
        name: 'MenuTree',
        props: {
            menu: Object,
            deep: {
                type: Number,
                default: 0
            }, // 当前的层级 从0开始
        },
        data() {
            return {
                isActive: false, // 当前菜单是否激活
                isLeaf: !this.menu.items || this.menu.items.length < 1, // 是否是叶子节点
                icons: ['home'],
            }
        },
        watch: {
            '$store.state.index.navMenueActive' (val) {
                this.setIsActive(val)
            }
        },
        created() {
            this.setIsActive(this.$store.state.index.navMenueActive)
        },
        activated() {
        },
        methods: {
            jumpUrl () {
                this.menu.url && this.$router.push({name: this.menu.name})
            },
            // 设置当前菜单是否激活
            setIsActive (activeMenu) {
                this.isActive = false
                activeMenu.forEach((item) => {
                    if (item.name === this.menu.name) {
                        this.isActive = true
                    }
                })
            }
        },
        components: {
            MenuTree
        }
    }
</script>
