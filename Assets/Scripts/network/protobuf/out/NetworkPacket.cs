// <auto-generated>
//     Generated by the protocol buffer compiler.  DO NOT EDIT!
//     source: NetworkPacket.proto
// </auto-generated>
#pragma warning disable 1591, 0612, 3021, 8981
#region Designer generated code

using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;
namespace Protobuf {

  /// <summary>Holder for reflection information generated from NetworkPacket.proto</summary>
  public static partial class NetworkPacketReflection {

    #region Descriptor
    /// <summary>File descriptor for NetworkPacket.proto</summary>
    public static pbr::FileDescriptor Descriptor {
      get { return descriptor; }
    }
    private static pbr::FileDescriptor descriptor;

    static NetworkPacketReflection() {
      byte[] descriptorData = global::System.Convert.FromBase64String(
          string.Concat(
            "ChNOZXR3b3JrUGFja2V0LnByb3RvEghwcm90b2J1ZiKHAQoNTmV0d29ya1Bh",
            "Y2tldBIMCgRkYXRhGAEgAigMEgwKBHR5cGUYAiACKAcSEAoIZGF0YVNpemUY",
            "AyACKAcSEQoJY2hhbm5lbElkGAQgAigJEhAKCGlkZW50aWZ5GAUgAigJEhEK",
            "CXRvdGFsU2l6ZRgGIAIoBxIQCghzZXF1ZW5jZRgHIAIoB0I2Ch1vcmcuZ29y",
            "YS5zZXJ2ZXIubW9kZWwubmV0d29ya0IVTmV0d29ya1BhY2tldFByb3RvQnVm"));
      descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
          new pbr::FileDescriptor[] { },
          new pbr::GeneratedClrTypeInfo(null, null, new pbr::GeneratedClrTypeInfo[] {
            new pbr::GeneratedClrTypeInfo(typeof(global::Protobuf.NetworkPacket), global::Protobuf.NetworkPacket.Parser, new[]{ "Data", "Type", "DataSize", "ChannelId", "Identify", "TotalSize", "Sequence" }, null, null, null, null)
          }));
    }
    #endregion

  }
  #region Messages
  public sealed partial class NetworkPacket : pb::IMessage<NetworkPacket>
  #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      , pb::IBufferMessage
  #endif
  {
    private static readonly pb::MessageParser<NetworkPacket> _parser = new pb::MessageParser<NetworkPacket>(() => new NetworkPacket());
    private pb::UnknownFieldSet _unknownFields;
    private int _hasBits0;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pb::MessageParser<NetworkPacket> Parser { get { return _parser; } }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public static pbr::MessageDescriptor Descriptor {
      get { return global::Protobuf.NetworkPacketReflection.Descriptor.MessageTypes[0]; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    pbr::MessageDescriptor pb::IMessage.Descriptor {
      get { return Descriptor; }
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public NetworkPacket() {
      OnConstruction();
    }

    partial void OnConstruction();

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public NetworkPacket(NetworkPacket other) : this() {
      _hasBits0 = other._hasBits0;
      data_ = other.data_;
      type_ = other.type_;
      dataSize_ = other.dataSize_;
      channelId_ = other.channelId_;
      identify_ = other.identify_;
      totalSize_ = other.totalSize_;
      sequence_ = other.sequence_;
      _unknownFields = pb::UnknownFieldSet.Clone(other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public NetworkPacket Clone() {
      return new NetworkPacket(this);
    }

    /// <summary>Field number for the "data" field.</summary>
    public const int DataFieldNumber = 1;
    private readonly static pb::ByteString DataDefaultValue = pb::ByteString.Empty;

    private pb::ByteString data_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public pb::ByteString Data {
      get { return data_ ?? DataDefaultValue; }
      set {
        data_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "data" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasData {
      get { return data_ != null; }
    }
    /// <summary>Clears the value of the "data" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearData() {
      data_ = null;
    }

    /// <summary>Field number for the "type" field.</summary>
    public const int TypeFieldNumber = 2;
    private readonly static uint TypeDefaultValue = 0;

    private uint type_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Type {
      get { if ((_hasBits0 & 1) != 0) { return type_; } else { return TypeDefaultValue; } }
      set {
        _hasBits0 |= 1;
        type_ = value;
      }
    }
    /// <summary>Gets whether the "type" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasType {
      get { return (_hasBits0 & 1) != 0; }
    }
    /// <summary>Clears the value of the "type" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearType() {
      _hasBits0 &= ~1;
    }

    /// <summary>Field number for the "dataSize" field.</summary>
    public const int DataSizeFieldNumber = 3;
    private readonly static uint DataSizeDefaultValue = 0;

    private uint dataSize_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint DataSize {
      get { if ((_hasBits0 & 2) != 0) { return dataSize_; } else { return DataSizeDefaultValue; } }
      set {
        _hasBits0 |= 2;
        dataSize_ = value;
      }
    }
    /// <summary>Gets whether the "dataSize" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasDataSize {
      get { return (_hasBits0 & 2) != 0; }
    }
    /// <summary>Clears the value of the "dataSize" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearDataSize() {
      _hasBits0 &= ~2;
    }

    /// <summary>Field number for the "channelId" field.</summary>
    public const int ChannelIdFieldNumber = 4;
    private readonly static string ChannelIdDefaultValue = "";

    private string channelId_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string ChannelId {
      get { return channelId_ ?? ChannelIdDefaultValue; }
      set {
        channelId_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "channelId" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasChannelId {
      get { return channelId_ != null; }
    }
    /// <summary>Clears the value of the "channelId" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearChannelId() {
      channelId_ = null;
    }

    /// <summary>Field number for the "identify" field.</summary>
    public const int IdentifyFieldNumber = 5;
    private readonly static string IdentifyDefaultValue = "";

    private string identify_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public string Identify {
      get { return identify_ ?? IdentifyDefaultValue; }
      set {
        identify_ = pb::ProtoPreconditions.CheckNotNull(value, "value");
      }
    }
    /// <summary>Gets whether the "identify" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasIdentify {
      get { return identify_ != null; }
    }
    /// <summary>Clears the value of the "identify" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearIdentify() {
      identify_ = null;
    }

    /// <summary>Field number for the "totalSize" field.</summary>
    public const int TotalSizeFieldNumber = 6;
    private readonly static uint TotalSizeDefaultValue = 0;

    private uint totalSize_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint TotalSize {
      get { if ((_hasBits0 & 4) != 0) { return totalSize_; } else { return TotalSizeDefaultValue; } }
      set {
        _hasBits0 |= 4;
        totalSize_ = value;
      }
    }
    /// <summary>Gets whether the "totalSize" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasTotalSize {
      get { return (_hasBits0 & 4) != 0; }
    }
    /// <summary>Clears the value of the "totalSize" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearTotalSize() {
      _hasBits0 &= ~4;
    }

    /// <summary>Field number for the "sequence" field.</summary>
    public const int SequenceFieldNumber = 7;
    private readonly static uint SequenceDefaultValue = 0;

    private uint sequence_;
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public uint Sequence {
      get { if ((_hasBits0 & 8) != 0) { return sequence_; } else { return SequenceDefaultValue; } }
      set {
        _hasBits0 |= 8;
        sequence_ = value;
      }
    }
    /// <summary>Gets whether the "sequence" field is set</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool HasSequence {
      get { return (_hasBits0 & 8) != 0; }
    }
    /// <summary>Clears the value of the "sequence" field</summary>
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void ClearSequence() {
      _hasBits0 &= ~8;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override bool Equals(object other) {
      return Equals(other as NetworkPacket);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public bool Equals(NetworkPacket other) {
      if (ReferenceEquals(other, null)) {
        return false;
      }
      if (ReferenceEquals(other, this)) {
        return true;
      }
      if (Data != other.Data) return false;
      if (Type != other.Type) return false;
      if (DataSize != other.DataSize) return false;
      if (ChannelId != other.ChannelId) return false;
      if (Identify != other.Identify) return false;
      if (TotalSize != other.TotalSize) return false;
      if (Sequence != other.Sequence) return false;
      return Equals(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override int GetHashCode() {
      int hash = 1;
      if (HasData) hash ^= Data.GetHashCode();
      if (HasType) hash ^= Type.GetHashCode();
      if (HasDataSize) hash ^= DataSize.GetHashCode();
      if (HasChannelId) hash ^= ChannelId.GetHashCode();
      if (HasIdentify) hash ^= Identify.GetHashCode();
      if (HasTotalSize) hash ^= TotalSize.GetHashCode();
      if (HasSequence) hash ^= Sequence.GetHashCode();
      if (_unknownFields != null) {
        hash ^= _unknownFields.GetHashCode();
      }
      return hash;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public override string ToString() {
      return pb::JsonFormatter.ToDiagnosticString(this);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void WriteTo(pb::CodedOutputStream output) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      output.WriteRawMessage(this);
    #else
      if (HasData) {
        output.WriteRawTag(10);
        output.WriteBytes(Data);
      }
      if (HasType) {
        output.WriteRawTag(21);
        output.WriteFixed32(Type);
      }
      if (HasDataSize) {
        output.WriteRawTag(29);
        output.WriteFixed32(DataSize);
      }
      if (HasChannelId) {
        output.WriteRawTag(34);
        output.WriteString(ChannelId);
      }
      if (HasIdentify) {
        output.WriteRawTag(42);
        output.WriteString(Identify);
      }
      if (HasTotalSize) {
        output.WriteRawTag(53);
        output.WriteFixed32(TotalSize);
      }
      if (HasSequence) {
        output.WriteRawTag(61);
        output.WriteFixed32(Sequence);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(output);
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalWriteTo(ref pb::WriteContext output) {
      if (HasData) {
        output.WriteRawTag(10);
        output.WriteBytes(Data);
      }
      if (HasType) {
        output.WriteRawTag(21);
        output.WriteFixed32(Type);
      }
      if (HasDataSize) {
        output.WriteRawTag(29);
        output.WriteFixed32(DataSize);
      }
      if (HasChannelId) {
        output.WriteRawTag(34);
        output.WriteString(ChannelId);
      }
      if (HasIdentify) {
        output.WriteRawTag(42);
        output.WriteString(Identify);
      }
      if (HasTotalSize) {
        output.WriteRawTag(53);
        output.WriteFixed32(TotalSize);
      }
      if (HasSequence) {
        output.WriteRawTag(61);
        output.WriteFixed32(Sequence);
      }
      if (_unknownFields != null) {
        _unknownFields.WriteTo(ref output);
      }
    }
    #endif

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public int CalculateSize() {
      int size = 0;
      if (HasData) {
        size += 1 + pb::CodedOutputStream.ComputeBytesSize(Data);
      }
      if (HasType) {
        size += 1 + 4;
      }
      if (HasDataSize) {
        size += 1 + 4;
      }
      if (HasChannelId) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(ChannelId);
      }
      if (HasIdentify) {
        size += 1 + pb::CodedOutputStream.ComputeStringSize(Identify);
      }
      if (HasTotalSize) {
        size += 1 + 4;
      }
      if (HasSequence) {
        size += 1 + 4;
      }
      if (_unknownFields != null) {
        size += _unknownFields.CalculateSize();
      }
      return size;
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(NetworkPacket other) {
      if (other == null) {
        return;
      }
      if (other.HasData) {
        Data = other.Data;
      }
      if (other.HasType) {
        Type = other.Type;
      }
      if (other.HasDataSize) {
        DataSize = other.DataSize;
      }
      if (other.HasChannelId) {
        ChannelId = other.ChannelId;
      }
      if (other.HasIdentify) {
        Identify = other.Identify;
      }
      if (other.HasTotalSize) {
        TotalSize = other.TotalSize;
      }
      if (other.HasSequence) {
        Sequence = other.Sequence;
      }
      _unknownFields = pb::UnknownFieldSet.MergeFrom(_unknownFields, other._unknownFields);
    }

    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    public void MergeFrom(pb::CodedInputStream input) {
    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
      input.ReadRawMessage(this);
    #else
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, input);
            break;
          case 10: {
            Data = input.ReadBytes();
            break;
          }
          case 21: {
            Type = input.ReadFixed32();
            break;
          }
          case 29: {
            DataSize = input.ReadFixed32();
            break;
          }
          case 34: {
            ChannelId = input.ReadString();
            break;
          }
          case 42: {
            Identify = input.ReadString();
            break;
          }
          case 53: {
            TotalSize = input.ReadFixed32();
            break;
          }
          case 61: {
            Sequence = input.ReadFixed32();
            break;
          }
        }
      }
    #endif
    }

    #if !GOOGLE_PROTOBUF_REFSTRUCT_COMPATIBILITY_MODE
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute]
    [global::System.CodeDom.Compiler.GeneratedCode("protoc", null)]
    void pb::IBufferMessage.InternalMergeFrom(ref pb::ParseContext input) {
      uint tag;
      while ((tag = input.ReadTag()) != 0) {
        switch(tag) {
          default:
            _unknownFields = pb::UnknownFieldSet.MergeFieldFrom(_unknownFields, ref input);
            break;
          case 10: {
            Data = input.ReadBytes();
            break;
          }
          case 21: {
            Type = input.ReadFixed32();
            break;
          }
          case 29: {
            DataSize = input.ReadFixed32();
            break;
          }
          case 34: {
            ChannelId = input.ReadString();
            break;
          }
          case 42: {
            Identify = input.ReadString();
            break;
          }
          case 53: {
            TotalSize = input.ReadFixed32();
            break;
          }
          case 61: {
            Sequence = input.ReadFixed32();
            break;
          }
        }
      }
    }
    #endif

  }

  #endregion

}

#endregion Designer generated code
