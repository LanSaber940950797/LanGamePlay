

# <strong>LanGamePlay

#### LanGamePlay是基于TEngine和GameNetty框架下的联网战斗通用框架。目前完成度60%
初衷是自己想做一款独立游戏，然后在TEenage的基于写了ECS的战斗框架，然后友人邀请做TPS联网同步游戏。就打算服务器使用ET，并将之前战斗框架代码用ET的ECS重构移植。


## <strong>主要特点
0. 基于TEngine+GameNetty框架，GameNetty是ET8的前后端分离版本，TEngine简单易用强大。ET十分强大，但要是严格按照ET写法会感觉比较复杂，所以打算用ET实现逻辑。其他等功能使用TEngine。怎么方便怎么来！！
1. 基于ET的ECS通用战斗框架。
2. 状态帧同步战斗框架，支持预测+回滚。
3. host-client模式联机




## 必要：项目使用了以下第三方插件，请自行购买导入：
   - /Unity/Assets/Plugins/Sirenix
## 该项目使用了以下收费插件：
- [DOTween Pro](https://assetstore.unity.com/packages/tools/visual-scripting/dotween-pro-32416) （简单易用强大的动画插件）
- [Odin Inspector](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041) （编辑器扩展、工作流改善）
- [Animancer Pro](https://assetstore.unity.com/packages/tools/animation/animancer-pro-116514) （基于Playable的简单强大的Animation解决方案）

## <strong>优质开源项目推荐

#### <a href="https://github.com/ALEXTANGXIAO/TEngine"><strong>TEngine</strong></a> -TEngine是一个简单(新手友好开箱即用)且强大的Unity框架全平台解决方案,对于需要一套上手快、文档清晰、高性能且可拓展性极强的商业级解决方案的开发者或者团队来说是一个很好的选择。
#### <a href="https://github.com/ALEXTANGXIAO/GameNetty"><strong>GameNetty</strong></a> - GameNetty是一套源于ETServer，首次拆分最新的ET8.1的前后端解决方案（包），客户端最精简大约750k，完美做成包的形式，几乎零成本 无侵入的嵌入进你的框架。
#### <a href="https://github.com/egametang/ET"><strong>ET</strong></a> - ET是一个开源的游戏客户端（基于unity3d）服务端双端框架，服务端是使用C# .net core开发的分布式游戏服务端，其特点是开发效率高，性能强，双端共享逻辑代码，客户端服务端热更机制完善。

#### <a href="https://github.com/m969/EGamePlay"><strong>EGamePlay</strong></a> - 通用战斗框架，战斗框架思路借鉴参考EGamePlay
#### <a href="https://github.com/AkiKurisu/AkiBT"><strong>AkiBT</strong></a> - 行为树
