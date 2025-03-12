#include "DataTypes.h"
namespace Cable::Data::Types::Native
{
	Geometry2D::Geometry2D(Cable::Data::Types::Geometry2D^ original)
	{
		for each (System::Numerics::Vector2 ^ vert in original->Vertices)
		{
			Vertices.push_back(XMFLOAT2(vert->X, vert->Y));
		}

		pin_ptr<int> pin(&original->Indices[0]);
		std::copy(
			static_cast<int*>(pin),
			static_cast<int*>(pin + original->Indices->Length),
			Indicies.begin()
		);
	}
}