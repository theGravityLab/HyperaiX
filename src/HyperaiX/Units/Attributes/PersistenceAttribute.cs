using System;

namespace HyperaiX.Units.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public class PersistenceAttribute : Attribute
{
    //TODO: 为具有该标记的 method 提供 Session 注入以支持多消息共享状态
}