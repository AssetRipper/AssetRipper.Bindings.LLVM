#ifndef LIBLLVMSHARP_LLVMSHARP_H
#define LIBLLVMSHARP_LLVMSHARP_H

#ifdef _MSC_VER
#pragma warning(push)
#pragma warning(disable : 4146 4244 4267 4291 4624 4996)
#endif

// Include headers
#include <clang-c/ExternC.h>
#include <llvm/IR/DerivedTypes.h>

#ifdef _MSC_VER
#pragma warning(pop)
#endif

#include "LLVM_export.h"

// Copied from the LLVMMetadataKind enum
#define LLVM_FOR_EACH_METADATA_SUBCLASS(macro) \
    macro(MDNode) \
    macro(DINode) \
    macro(DIScope) \
    macro(DITemplateParameter) \
    macro(DIType) \
    macro(DIVariable) \
    macro(MDString) \
    macro(ConstantAsMetadata) \
    macro(LocalAsMetadata) \
    macro(DistinctMDOperandPlaceholder) \
    macro(MDTuple) \
    macro(DILocation) \
    macro(DIExpression) \
    macro(DIGlobalVariableExpression) \
    macro(GenericDINode) \
    macro(DISubrange) \
    macro(DIEnumerator) \
    macro(DIBasicType) \
    macro(DIDerivedType) \
    macro(DICompositeType) \
    macro(DISubroutineType) \
    macro(DIFile) \
    macro(DICompileUnit) \
    macro(DISubprogram) \
    macro(DILexicalBlock) \
    macro(DILexicalBlockFile) \
    macro(DINamespace) \
    macro(DIModule) \
    macro(DITemplateTypeParameter) \
    macro(DITemplateValueParameter) \
    macro(DIGlobalVariable) \
    macro(DILocalVariable) \
    macro(DILabel) \
    macro(DIObjCProperty) \
    macro(DIImportedEntity) \
    macro(DIMacro) \
    macro(DIMacroFile) \
    macro(DICommonBlock) \
    macro(DIStringType) \
    macro(DIGenericSubrange) \
    macro(DIArgList) \
    macro(DIAssignID) \

/**
 * Represents an individual value in LLVM IR.
 *
 * This models llvm::Value.
 */
typedef struct LLVMOpaquePass* LLVMPassRef;

// Enum definitions

// Struct definitions

LLVM_CLANG_C_EXTERN_C_BEGIN

// Function declarations

LLVM_API const char* llvmsharp_ConstantDataArray_getData(LLVMValueRef array, int32_t* out_size);

LLVM_API uint32_t llvmsharp_DIBasicType_getEncoding(LLVMMetadataRef type);

LLVM_API LLVMMetadataRef llvmsharp_DICompositeType_getBaseType(LLVMMetadataRef type);

LLVM_API void llvmsharp_DICompositeType_getElements(LLVMMetadataRef type, LLVMMetadataRef** out_buffer, int32_t* out_size);

LLVM_API const char* llvmsharp_DICompositeType_getIdentifier(LLVMMetadataRef type, int32_t* out_size);

LLVM_API LLVMMetadataRef llvmsharp_DIDerivedType_getBaseType(LLVMMetadataRef type);

LLVM_API LLVMMetadataRef llvmsharp_DIDerivedType_getExtraData(LLVMMetadataRef type);

LLVM_API const char* llvmsharp_DIEnumerator_getName(LLVMMetadataRef enumerator, int32_t* out_size);

LLVM_API int64_t llvmsharp_DIEnumerator_getValue_SExt(LLVMMetadataRef enumerator);

LLVM_API uint64_t llvmsharp_DIEnumerator_getValue_ZExt(LLVMMetadataRef enumerator);

LLVM_API uint8_t llvmsharp_DIEnumerator_isUnsigned(LLVMMetadataRef enumerator);

LLVM_API LLVMMetadataRef llvmsharp_DIImportedEntity_getEntity(LLVMMetadataRef node);

LLVM_API LLVMMetadataRef llvmsharp_DIImportedEntity_getFile(LLVMMetadataRef node);

LLVM_API uint32_t llvmsharp_DIImportedEntity_getLine(LLVMMetadataRef node);

LLVM_API LLVMMetadataRef llvmsharp_DIImportedEntity_getScope(LLVMMetadataRef node);

LLVM_API uint32_t llvmsharp_DILexicalBlock_getLine(LLVMMetadataRef block);

LLVM_API LLVMMetadataRef llvmsharp_DILexicalBlock_getScope(LLVMMetadataRef block);

LLVM_API const char* llvmsharp_DINamespace_getName(LLVMMetadataRef node, int32_t* out_size);

LLVM_API LLVMMetadataRef llvmsharp_DINamespace_getScope(LLVMMetadataRef node);

LLVM_API const char* llvmsharp_DINode_getTagString(LLVMMetadataRef node, int32_t* out_size);

LLVM_API LLVMMetadataRef llvmsharp_DISubprogram_getContainingType(LLVMMetadataRef subprogram);

LLVM_API uint32_t llvmsharp_DISubprogram_getFlags(LLVMMetadataRef subprogram);

LLVM_API const char* llvmsharp_DISubprogram_getLinkageName(LLVMMetadataRef subprogram, int32_t* out_size);

LLVM_API const char* llvmsharp_DISubprogram_getName(LLVMMetadataRef subprogram, int32_t* out_size);

LLVM_API uint32_t llvmsharp_DISubprogram_getScopeLine(LLVMMetadataRef subprogram);

LLVM_API uint32_t llvmsharp_DISubprogram_getSPFlags(LLVMMetadataRef subprogram);

LLVM_API void llvmsharp_DISubprogram_getTemplateParams(LLVMMetadataRef subprogram, LLVMMetadataRef** out_buffer, int32_t* out_size);

LLVM_API LLVMMetadataRef llvmsharp_DISubprogram_getType(LLVMMetadataRef subprogram);

LLVM_API uint32_t llvmsharp_DISubprogram_getVirtualIndex(LLVMMetadataRef subprogram);

LLVM_API LLVMValueRef llvmsharp_DISubrange_getCount(LLVMMetadataRef subrange);

LLVM_API void llvmsharp_DISubroutineType_getTypeArray(LLVMMetadataRef subroutine_type, LLVMMetadataRef** out_buffer, int32_t* out_size);

LLVM_API LLVMMetadataRef llvmsharp_DITemplateParameter_getType(LLVMMetadataRef parameter);

LLVM_API LLVMMetadataRef llvmsharp_DITemplateValueParameter_getValue(LLVMMetadataRef parameter);

LLVM_API const char* llvmsharp_DIVariable_getName(LLVMMetadataRef variable, int32_t* out_size);

LLVM_API LLVMMetadataRef llvmsharp_DIVariable_getType(LLVMMetadataRef variable);

LLVM_API LLVMTypeRef llvmsharp_Function_getFunctionType(LLVMValueRef function);

LLVM_API LLVMTypeRef llvmsharp_Function_getReturnType(LLVMValueRef function);

LLVM_API LLVMMetadataRef llvmsharp_GlobalVariable_getGlobalVariableExpression(LLVMValueRef global_variable);

LLVM_API LLVMMetadataRef llvmsharp_GlobalVariable_getMetadata(LLVMValueRef global_variable, uint32_t KindID);

LLVM_API uint8_t llvmsharp_Instruction_hasNoSignedWrap(LLVMValueRef instruction);

LLVM_API uint8_t llvmsharp_Instruction_hasNoUnsignedWrap(LLVMValueRef instruction);

LLVM_API uint32_t llvmsharp_MDNode_getNumOperands(LLVMMetadataRef metadata);

LLVM_API LLVMMetadataRef llvmsharp_MDNode_getOperand(LLVMMetadataRef metadata, uint32_t index);

LLVM_API const char* llvmsharp_MDString_getString(LLVMMetadataRef mdstring, int32_t* out_size);

#define LLVMSHARP_METADATA_ISA(CPP_TYPE) LLVM_API LLVMMetadataRef llvmsharp_Metadata_IsA##CPP_TYPE(LLVMMetadataRef metadata);

LLVM_FOR_EACH_METADATA_SUBCLASS(LLVMSHARP_METADATA_ISA)

#undef LLVMSHARP_METADATA_ISA

LLVM_API void llvmsharp_Module_GetIdentifiedStructTypes(LLVMModuleRef module, LLVMTypeRef** out_buffer, int32_t* out_size);

LLVM_API void llvmsharp_PassManager_add(LLVMPassManagerRef pass_manager, LLVMPassRef pass);

LLVM_API LLVMPassRef llvmsharp_createDeadCodeEliminationPass();

LLVM_API LLVMPassRef llvmsharp_createSROAPass(uint8_t PreserveCFG);

LLVM_API LLVMPassRef llvmsharp_createLICMPass();

LLVM_API LLVMPassRef llvmsharp_createLoopStrengthReducePass();

LLVM_API LLVMPassRef llvmsharp_createReassociatePass();

LLVM_API LLVMPassRef llvmsharp_createFlattenCFGPass();

LLVM_API LLVMPassRef llvmsharp_createCFGSimplificationPass();

LLVM_API LLVMPassRef llvmsharp_createTailCallEliminationPass();

LLVM_API LLVMPassRef llvmsharp_createConstantHoistingPass();

LLVM_API LLVMPassRef llvmsharp_createLowerInvokePass();

LLVM_API LLVMPassRef llvmsharp_createLowerSwitchPass();

LLVM_API LLVMPassRef llvmsharp_createBreakCriticalEdgesPass();

LLVM_API LLVMPassRef llvmsharp_createLCSSAPass();

LLVM_API LLVMPassRef llvmsharp_createPromoteMemoryToRegisterPass();

LLVM_API LLVMPassRef llvmsharp_createLoopSimplifyPass();

LLVM_API LLVMPassRef llvmsharp_createUnifyLoopExitsPass();

LLVM_API int32_t llvmsharp_Demangle(const char* mangled_string, int32_t mangled_string_size, char* buffer, int32_t buffer_size);

LLVM_API void llvmsharp_Free(void* obj);

LLVM_CLANG_C_EXTERN_C_END

#endif